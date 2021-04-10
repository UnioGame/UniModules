using System;
using Cysharp.Threading.Tasks;
using UniModules.UniGame.Rx.Runtime.Extensions;
using UniRx;
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
  
        public static IDisposable BindChangeHandler(this LocalizedString source, LocalizedString.ChangeHandler handler,int frameThrottle = 1)
        {
            return BindTo(source,handler,frameThrottle);          
        }
            
        public static IDisposable BindTo(this LocalizedString source, LocalizedString.ChangeHandler handler,int frameThrottle = 1)
        {
            if(source == null || handler == null)
                return Disposable.Empty;
            
            var result = Observable
                .FromEvent(x => source.StringChanged += handler, x => source.StringChanged -= handler)
                .BatchPlayerTiming(frameThrottle,PlayerLoopTiming.LastPostLateUpdate)
                .Subscribe();
            return result;          
        }

        public static IObservable<string> AsObservable(this LocalizedString localizedString)
        {
            return Observable.Create<string>(x => localizedString.BindTo(x.OnNext));
        }
    }
}
