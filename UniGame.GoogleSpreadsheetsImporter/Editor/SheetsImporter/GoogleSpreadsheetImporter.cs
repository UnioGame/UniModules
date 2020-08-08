namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using EditorWindow;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Util.Store;
    using GoogleSpreadsheets.Editor.SheetsImporter;
    using TypeConverters.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = "UniGame/Google/GoogleSpreadSheetImporter",fileName = nameof(GoogleSpreadsheetImporter))]
    public class GoogleSpreadsheetImporter : ScriptableObject, ILifeTimeContext
    {
        private const int DefaultButtonsWidth = 60;

        #region inspector
        
        /// <summary>
        /// credential profile file
        /// https://developers.google.com/sheets/api/quickstart/dotnet
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup(nameof(user),false)]
        [Sirenix.OdinInspector.FilePath]
#endif
        public string credentialsPath;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup(nameof(user))]
#endif
        public string user = "user";
        
        /// <summary>
        /// list of target sheets
        /// </summary>
        [Space(4)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup("Sheets")]
        [Sirenix.OdinInspector.BoxGroup("Sheets/Sheets Ids",false)]
#endif
        public List<string> sheetsIds = new List<string>();
        
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
        [Space(8)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty()]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.HorizontalGroup("Sources")]
        [Sirenix.OdinInspector.BoxGroup("Sources/Synced Assets")]
#endif
        public SpreadsheetImportersHandler sheetsItemsHandler = new SpreadsheetImportersHandler();
        
        [Space(8)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorObjectFieldModes.Boxed ,Expanded = true)]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.BoxGroup("Converters")]
#endif
        public ObjectTypeConverter typeConverters;
        
        #endregion

        #region private data

        private LifeTimeDefinition _lifeTime;
        
        private SpreadsheetData _spreadsheetData = new SpreadsheetData();
        
        private List<GoogleSpreadsheetClient> _sheetClients = new List<GoogleSpreadsheetClient>();

        private SheetsService _sheetService;

        #endregion
        
        #region public properties

        public SheetsService SheetsService {
            get {
                _sheetService = _sheetService ?? 
                                 LoadSheetService(GoogleSheetImporterConstants.ApplicationName,GoogleSpreadsheetClient.WriteScope);
                LifeTime.AddCleanUpAction(() => _sheetService = null);
                return _sheetService;
            }
        }
            
        
        public SpreadsheetData SpreadsheetData {
            get => _spreadsheetData;
            set => _spreadsheetData = value;
        }
        
        public ILifeTime LifeTime => (_lifeTime = _lifeTime == null ? new LifeTimeDefinition() : _lifeTime);
        
        #endregion

        #region public methods
        
        public void ReloadSpreadsheetData()
        {
            _lifeTime?.Release();

            CreateSheetClients();
            LoadTypeConverters();
        }
        
        [Sirenix.OdinInspector.HorizontalGroup("Sources",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.VerticalGroup("Sources/Source Commands",PaddingTop = 20)]
        [Sirenix.OdinInspector.Button("Reload")]
        public void ReloadSyncedAssets()
        {
            sheetsItemsHandler.Load();
            this.MarkDirty();
        }
        
        [Sirenix.OdinInspector.VerticalGroup("Sources/Source Commands")]
        [Sirenix.OdinInspector.Button("Import")]
        public void Import()
        {
            sheetsItemsHandler.Import(SpreadsheetData);
        }
        
        [Sirenix.OdinInspector.VerticalGroup("Sources/Source Commands")]
        [Sirenix.OdinInspector.Button("Export")]
        public void Export()
        {
            SpreadsheetData = sheetsItemsHandler.Export(SpreadsheetData);
            foreach (var sheetData in SpreadsheetData.Sheets) {
                foreach (var client in _sheetClients) {
                    if(!client.HasSheet(sheetData.Id))
                        continue;
                    client.UpdateData(sheetData);
                }
            }
        }

        [Sirenix.OdinInspector.HorizontalGroup("Sheets",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.BoxGroup("Sheets/Commands",false)]
        [Sirenix.OdinInspector.Button("Show")]
        public void ShowSpreadSheets()
        {
            GoogleSpreadSheetViewWindow.Open(_sheetClients);
        }

        [Sirenix.OdinInspector.Button("Reset Credentials")]
        public void ResetCredentials()
        {
            if(Directory.Exists(GoogleSheetImporterConstants.TokenKey))
                Directory.Delete(GoogleSheetImporterConstants.TokenKey,true);
            _lifeTime?.Release();
            
        }
        
        public List<Object> LoadSyncAssets()
        {
            return new List<Object>();
        }

        public void CreateSyncAssets()
        {
            
        }
        
        #endregion

        #region private methods

        private void LoadTypeConverters()
        {
            typeConverters = typeConverters ? typeConverters : ObjectTypeConverter.TypeConverters;
        }
        
        private void OnEnable()
        {
            LoadTypeConverters();
        }

        private void CreateSheetClients()
        {
            foreach (var sheetsId in sheetsIds.Distinct()) {
                if(_sheetClients.Any(x => x.Id == sheetsId))
                    continue;
                var client = new GoogleSpreadsheetClient(SheetsService,sheetsId);
                client.Reload();
                _sheetClients.Add(client);
            }

            _spreadsheetData.Initialize(_sheetClients);
            
            LifeTime.AddCleanUpAction(_sheetClients.Clear);
        }

        /// <summary>
        /// create sheet service
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="scope">target and permissions scope. User GoogleSheetClient.*[Scope] constants</param>
        /// <returns></returns>
        private SheetsService LoadSheetService(string applicationName,string[] scope)
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                var cancelationSource = new CancellationTokenSource();
                cancelationSource.CancelAfter(TimeSpan.FromSeconds(30f));
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scope,
                    user,
                    cancelationSource.Token,
                    new FileDataStore(GoogleSheetImporterConstants.TokenKey, true)).
                    Result;
                Console.WriteLine("Credential file saved to: " + GoogleSheetImporterConstants.TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(
                new BaseClientService.Initializer() {
                    HttpClientInitializer = credential,
                    ApplicationName       = applicationName,
                }).AddTo(LifeTime);

            return service;
        }

        #endregion

    }
}
