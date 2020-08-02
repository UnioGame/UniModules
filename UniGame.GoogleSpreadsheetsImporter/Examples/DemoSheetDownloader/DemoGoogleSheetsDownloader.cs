namespace UniModules.UniGame.GoogleSpreadsheets.Examples.DemoSheetDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Google.Apis.Util.Store;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Examples/DemoGoogleSheetsDownloader", fileName = nameof(DemoGoogleSheetsDownloader))]
    public class DemoGoogleSheetsDownloader : ScriptableObject
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes          = { SheetsService.Scope.SpreadsheetsReadonly };
        static string   ApplicationName = "UniGame Google Sheets Importer";
        static string   TokenKey = "token.json";

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FilePath]
#endif     
        public string credentialsPath;

        public string sheedId = "1yFtDRq7BSKQu-EHOOIJj9d7BZhU7noo7xYg-ShKABM0";
        
        /// <summary>
        /// Class Data - sheet name
        /// !A2:H - data range
        /// </summary>
        public string table = "Class Data";
        public string writeTable = "Class Data2";

        public string range = "!A2:H";
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Upload()
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] {
                        "https://spreadsheets.google.com/feeds",
                        SheetsService.Scope.Spreadsheets,
                        SheetsService.Scope.Drive,
                    },
                    "mefodei.link@gmail.com",
                    CancellationToken.None,
                    new FileDataStore(TokenKey, true)).Result;
                Console.WriteLine("Credential file saved to: " + TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName       = ApplicationName,
            });

            var valueRange = new ValueRange() {
                Values = new List<IList<object>>() {
                    new List<object>() {
                        "111","222","333","444","555"
                    }
                },
                Range = writeTable
            };
            var request = service.Spreadsheets.Values.Update(valueRange, sheedId, writeTable);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            request.Execute();
            
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Download()
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(TokenKey, true)).Result;
                Console.WriteLine("Credential file saved to: " + TokenKey);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            var spreadsheetId = sheedId;
            var targetRange = table + range;
            var request = service.Spreadsheets.Values.Get(spreadsheetId, targetRange);

            var response = request.Execute();
            var values = response.Values;
            var valuesMessage = string.Empty;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values) {
                    valuesMessage += string.Join(" ", row);
                    valuesMessage += "\n";
                }
            }
            else
            {
                Debug.Log("No data found.");
            }
            
            Debug.Log(valuesMessage);
        }
    }
}

