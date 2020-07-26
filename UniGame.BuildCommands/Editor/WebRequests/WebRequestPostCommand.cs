namespace UniModules.UniGame.BuildCommands.Editor.WebRequests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using UniBuild.Editor.ClientBuild.Commands.PostBuildCommands;
    using UniBuild.Editor.ClientBuild.Interfaces;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UnityEditor.Build.Reporting;
    using UnityEngine;
    using UnityEngine.Networking;

    [CreateAssetMenu(menuName = "UniGame/UniBuild/Commands/WebRequest Post", fileName = nameof(WebRequestPostCommand))]
    public class WebRequestPostCommand : UnityPostBuildCommand
    {
        public string apiUrl = "";

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Parameters")]
#endif
        public WebRequestParameters header = new WebRequestParameters() {
            {"Content-Type","application/json"},
            {"Accept","application/json"},
        };
        
        [Space(4)]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.BoxGroup("Parameters")]
#endif
        public WebRequestParameters parameters = new WebRequestParameters();
        
        public override void Execute(IUniBuilderConfiguration configuration, BuildReport buildReport) => Execute();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Execute()
        {
            var urlParameters = new List<string>();
            foreach (var parameter in parameters) {
                var keyValue = $"{parameter.Key}={UnityWebRequest.EscapeURL(parameter.Value)}";
                urlParameters.Add(keyValue);
            }

            var targetUrl = apiUrl;
            if (urlParameters.Count > 0) {
                targetUrl += $"?{string.Join("&",urlParameters)}";
            }
            
            var webRequest = UnityWebRequest.Post(targetUrl,string.Empty);
            foreach (var headerParameter in header) {
                webRequest.SetRequestHeader(headerParameter.Key,headerParameter.Value);
            }
            
            var reqeustValue = webRequest.uri;
            Debug.Log($"Send Post to : {reqeustValue}");
            
            var requestAsyncOperation = webRequest.SendWebRequest();
            requestAsyncOperation.completed += x => {
                
                if (webRequest.isNetworkError || webRequest.isHttpError) {
                    Debug.Log(webRequest.error);
                }
                else {
                    Debug.Log($"Request to {apiUrl} complete. Code: {webRequest.responseCode}");
                }

                webRequest.Cancel();
            };
        }
    }
}
