using UnityEngine;

namespace UniModules.UniGame.GoogleSpreadsheets.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Core.Runtime.ScriptableObjects;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Util.Store;
    using Runtime.Attributes;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = "UniGame/Google/GoogleSheetImporter",fileName = nameof(GoogleSheetImporter))]
    public class GoogleSheetImporter : LifetimeScriptableObject
    {

        #region inspector
        
        /// <summary>
        /// credential profile file
        /// https://developers.google.com/sheets/api/quickstart/dotnet
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FilePath]
#endif
        public string credentialsPath;

        public string user = "user";
        
        /// <summary>
        /// list of target sheets
        /// </summary>
        public List<string> sheetsIds = new List<string>();
        
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<Object> autoLinkedAssets = new List<Object>();
        
        #endregion

        #region private data
        
        private List<GoogleSheetClient> sheetClients = new List<GoogleSheetClient>();

        private SheetsService _sheetService;
        
        #endregion
        
        #region public properties

        public SheetsService SheetsService => (_sheetService = _sheetService ?? LoadSheetService());
        
        #endregion

        public List<Object> LoadSyncAssets()
        {
            return new List<Object>();
        }

        public void CreateSyncAssets()
        {
            
        }

        #region private methods

        private void CreateSheetClients()
        {
            foreach (var sheetsId in sheetsIds.Distinct()) {
                if(sheetClients.Any(x => x.Id == sheetsId))
                    continue;
                var client = new GoogleSheetClient(SheetsService,sheetsId);
                sheetClients.Add(client);
            }
        }

        private List<Object> LoadSyncScriptableObjects()
        {
            var assets = AssetEditorTools.
                GetAssetsWithAttribute<ScriptableObject,SheetItemAttribute>();
            return null;
        }

        private SheetsService LoadSheetService()
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    GoogleSheetClient.WriteScope,
                    user,
                    CancellationToken.None,
                    new FileDataStore(GoogleSheetImporterConstants.TokenKey, true)).
                    Result;
                Console.WriteLine("Credential file saved to: " + GoogleSheetImporterConstants.TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName       = GoogleSheetImporterConstants.ApplicationName,
            });

            return service;
        }
        
        #endregion
    }
}
