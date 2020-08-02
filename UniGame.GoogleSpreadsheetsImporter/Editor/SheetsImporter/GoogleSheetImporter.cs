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
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = "UniGame/Google/GoogleSheetImporter",fileName = nameof(GoogleSheetImporter))]
    public class GoogleSheetImporter : ScriptableObject, ILifeTimeContext
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
        public SheetsItemsSource sheetsItemsSource = new SheetsItemsSource();
        
        #endregion

        #region private data

        private LifeTimeDefinition _lifeTime;
        
        private List<GoogleSpreadsheetClient> _sheetClients = new List<GoogleSpreadsheetClient>();

        private SheetsService _sheetService;

        #endregion
        
        #region public properties

        public SheetsService ReadonlySheetsService => 
            (_sheetService = _sheetService ?? 
                             LoadSheetService(GoogleSheetImporterConstants.ApplicationName,GoogleSpreadsheetClient.ReadonlyScopes));


        public ILifeTime LifeTime => (_lifeTime = _lifeTime == null ? new LifeTimeDefinition() : _lifeTime);
        
        #endregion

        #region public methods
        
        public void Initialize()
        {
            _lifeTime?.Release();

            CreateSheetClients();
        }
        
        [Sirenix.OdinInspector.HorizontalGroup("Sources",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.BoxGroup("Sources/Source Commands",false)]
        [Sirenix.OdinInspector.Button("Reload")]
        public void ReloadSyncedAssets()
        {
            sheetsItemsSource.Reload();
            this.MarkDirty();
        }
        
        [Sirenix.OdinInspector.BoxGroup("Sources/Source Commands")]
        [Sirenix.OdinInspector.Button("Import")]
        public void ImportFromRemote()
        {
            
        }

        [Sirenix.OdinInspector.HorizontalGroup("Sheets",DefaultButtonsWidth)]
        [Sirenix.OdinInspector.BoxGroup("Sheets/Commands",false)]
        [Sirenix.OdinInspector.Button("Show")]
        public void ShowSpreadSheets()
        {
            GoogleSpreadSheetViewWindow.Open(_sheetClients);
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

        private void CreateSheetClients()
        {
            foreach (var sheetsId in sheetsIds.Distinct()) {
                if(_sheetClients.Any(x => x.Id == sheetsId))
                    continue;
                var client = new GoogleSpreadsheetClient(ReadonlySheetsService,sheetsId);
                client.Reload();
                _sheetClients.Add(client);
            }

            LifeTime.AddCleanUpAction(_sheetClients.Clear);
        }
        
        /// <summary>
        /// create sheet service
        /// </summary>d
        /// <param name="scope">target and permitions scope. User GoogleSheetClient.*[Scope] constants</param>
        /// <returns></returns>
        private SheetsService LoadSheetService(string applicationName,string[] scope)
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scope,
                    user,
                    CancellationToken.None,
                    new FileDataStore(GoogleSheetImporterConstants.TokenKey, true)).
                    Result;
                Console.WriteLine("Credential file saved to: " + GoogleSheetImporterConstants.TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName       = applicationName,
            });

            return service;
        }

        #endregion

    }
}
