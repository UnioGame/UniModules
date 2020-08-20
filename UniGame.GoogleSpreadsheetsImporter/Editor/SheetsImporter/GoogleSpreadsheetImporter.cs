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
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = "UniGame/Google/GoogleSpreadSheetImporter",fileName = nameof(GoogleSpreadsheetImporter))]
    public class GoogleSpreadsheetImporter : ScriptableObject, ILifeTimeContext
    {
        private const int DefaultButtonsWidth = 100;

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
        [Sirenix.OdinInspector.InfoBox("Add any valid spreadsheet id's")]
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

        private bool _isConnectionRefused = true;
        
        private LifeTimeDefinition _lifeTime;
        
        private SpreadsheetData _spreadsheetData = new SpreadsheetData();
        
        private List<GoogleSpreadsheetClient> _sheetClients = new List<GoogleSpreadsheetClient>();

        private SheetsService _sheetService;

        #endregion
        
        #region public properties

        public bool IsValidToConnect => sheetsIds.Any(x => !string.IsNullOrEmpty(x));

        public bool HasConnectedSheets => SpreadsheetData != null && SpreadsheetData.Sheets.Count > 0 && _sheetClients.Count > 0;
        
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
        
        public ILifeTime LifeTime => (_lifeTime = _lifeTime ?? new LifeTimeDefinition());
        
        #endregion

        #region public methods
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup("Sources",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.VerticalGroup("Sources/Source Commands",PaddingTop = 30)]
        [Sirenix.OdinInspector.Button("Import All")]
        [Sirenix.OdinInspector.EnableIf("HasConnectedSheets")]
#endif
        public void Import()
        {
            sheetsItemsHandler.Import();
        }
       
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.VerticalGroup("Sources/Source Commands")]
        [Sirenix.OdinInspector.Button("Export All")]
        [Sirenix.OdinInspector.EnableIf("HasConnectedSheets")]
#endif
        public void Export()
        {
            SpreadsheetData = sheetsItemsHandler.Export();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup("Sheets",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.BoxGroup("Sheets/Commands",false)]
        [Sirenix.OdinInspector.Button("Show")]
#endif
        public void ShowSpreadSheets()
        {
            GoogleSpreadSheetViewWindow.Open(_sheetClients);
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button("Reload Spreadsheets")]   
        [Sirenix.OdinInspector.EnableIf("HasConnectedSheets")]
#endif
        private void ReloadSpreadsheetsData()
        {
            _sheetClients.ForEach(x=> x.Reload());
            _spreadsheetData.Initialize(_sheetClients.SelectMany(x => x.GetAllSheetsData()));
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.EnableIf("IsValidToConnect")]
        [Sirenix.OdinInspector.Button("Connect Spreadsheets")]    
#endif
        public void ConnectSpreadsheets()
        {
            _lifeTime?.Release();
            CreateSheetClients();
            LoadTypeConverters();
            ReloadSpreadsheetsData();
            sheetsItemsHandler.Initialize(_spreadsheetData,_sheetClients);
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ButtonGroup()]
        [Sirenix.OdinInspector.Button("Reset Credentials")]
#endif
        public void ResetCredentials()
        {
            if(Directory.Exists(GoogleSheetImporterConstants.TokenKey))
                Directory.Delete(GoogleSheetImporterConstants.TokenKey,true);
            _lifeTime?.Release();
        }

        #endregion

        #region private methods

        private void LoadTypeConverters()
        {
            typeConverters = typeConverters ? typeConverters : ObjectTypeConverter.TypeConverters;
        }
        
        private void OnEnable()
        {
            _sheetClients = new List<GoogleSpreadsheetClient>();
            LoadTypeConverters();
        }

        private void CreateSheetClients()
        {
            foreach (var sheetsId in sheetsIds.Distinct()) {
                if(_sheetClients.Any(x => x.Id == sheetsId))
                    continue;
                var client = new GoogleSpreadsheetClient(SheetsService,sheetsId);
                _sheetClients.Add(client);
            }

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
                Debug.Log("Credential file saved to: " + GoogleSheetImporterConstants.TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(
                new BaseClientService.Initializer() {
                    HttpClientInitializer = credential,
                    ApplicationName       = applicationName,
                }).AddTo(LifeTime);
            
            _isConnectionRefused = false;
            
            LifeTime.AddCleanUpAction(() => _isConnectionRefused = true);
            
            return service;
        }

        #endregion

    }
}
