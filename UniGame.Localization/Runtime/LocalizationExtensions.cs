using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Utils;
using UniModules.UniGame.Rx.Runtime.Extensions;
using UniModules.UniUiSystem.Runtime.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UniGame.Localization.Runtime.UniModules.UniGame.Localization.Runtime
{
    public static class LocalizationExtensions
    {
        public static LocalizedString ToLocalizedString(this string key)
        {
            var splittedKey = key.Split('/');
            if (splittedKey.Length < 2)
                return null;

            var table = splittedKey[0];
            var entry = splittedKey[1];

            var localizedString = new LocalizedString();
            localizedString.SetReference(table, entry);

            return localizedString;
        }

        public static LocalizedString ToLocalizedString(this TableEntryReference reference)
        {
            var defaultTable = LocalizationSettings.StringDatabase.DefaultTable;
            return ToLocalizedString(reference, defaultTable);
        }

        public static LocalizedString ToLocalizedString(this TableEntryReference reference, TableReference tableEntry)
        {
            var result = new LocalizedString();
            result.SetReference(tableEntry, reference);
            return result;
        }
        
        public static IDisposable BindChangeHandler(this LocalizedString source, IObserver<string> handler,int frameThrottle = 1)
        {
            return BindTo(source,x => handler?.OnNext(x),frameThrottle);          
        }
  
        public static IDisposable BindChangeHandler(this LocalizedString source, Action<string> handler,int frameThrottle = 1)
        {
            return BindTo(source,handler,frameThrottle);          
        }

        public static IDisposable BindTo(this LocalizedString source, TextMeshProUGUI text, int frameThrottle = 1)
        {
            return source.BindTo(x => text.SetValue(x),frameThrottle);
        }
        
        public static IDisposable BindTo(this LocalizedString source, Action<string> text, int frameThrottle = 1)
        {
            var result = Observable
                .Create<string>(x => BindTo(source, x, frameThrottle),true)
                .Do(x => text?.Invoke(x))
                .Subscribe();
            
            return result;
        }
        
        public static IDisposable BindTo(this LocalizedString source, IReactiveProperty<string> text, int frameThrottle = 1)
        {
            var result = Observable
                .Create<string>(x => BindTo(source, x, frameThrottle),true)
                .Do(x => text.Value = x)
                .Subscribe();
            
            return result;
        }
        
        public static IDisposable BindTo(this LocalizedString source, IObserver<string> action,int frameThrottle = 1)
        {
            if(source == null || action == null)
                return Disposable.Empty;

            var result = source
                .AsObservable()
                .Do(x =>  Debug.Log($"LOCALIZATION DATA {x} STATE {UniApplication.PlayModeState}"))
                .BatchPlayerTiming(frameThrottle,PlayerLoopTiming.LastPostLateUpdate)
                .Subscribe(action);

            source.RefreshString();
            
            return result;          
        }

        public static IObservable<string> AsObservable(this LocalizedString localizedString)
        {
            return Observable.FromEvent<string>(y  => localizedString.StringChanged += y.Invoke,
                y => localizedString.StringChanged -= y.Invoke);
        }

    }
}
