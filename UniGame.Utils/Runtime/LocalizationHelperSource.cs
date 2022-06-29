namespace UniGame.Utils.Runtime
{
    using Cysharp.Threading.Tasks;
    using UniGame.Localization.Runtime.UniModules.UniGame.Localization.Runtime;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGame.SerializableContext.Runtime.Addressables;
    using UniModules.UniGameFlow.GameFlow.Runtime.Services;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Localization.Settings;
    using UnityEngine.Localization.SmartFormat.Utilities;

    [CreateAssetMenu(menuName = "Taktika/Sources/LocalizationHelperSource", fileName = nameof(LocalizationHelperSource))]
    public class LocalizationHelperSource : ServiceDataSourceAsset
    {
        [SerializeField] private AssetReferenceT<TimeFormatSettings> _timeTextInfoSource;

        protected override async UniTask<IEmptyGameService> CreateServiceInternalAsync(IContext context)
        {
            var timeTextInfo = await _timeTextInfoSource.LoadAssetTaskAsync(LifeTime);

            timeTextInfo.Seconds.AsObservable().CombineLatest(
                timeTextInfo.Minutes.AsObservable().Where(x => x != null),
                timeTextInfo.Hours.AsObservable().Where(x => x != null),
                timeTextInfo.Days.AsObservable().Where(x => x != null),
                timeTextInfo.Weeks.AsObservable().Where(x => x != null),
                timeTextInfo.LessThan.AsObservable().Where(x => x != null),
                SetTimeTextInfo)
                .Subscribe()
                .AddTo(LifeTime);

            return null;
        }

        private Unit SetTimeTextInfo(string seconds, string minutes, string hours, string days, string weeks, string lessThan)
        {
            var selectedLocale = LocalizationSettings.SelectedLocale.Identifier.Code;
            LocalizationHelper.LocalizedTimeTextInfo = new TimeTextInfo(
                PluralRules.GetPluralRule(selectedLocale),
                new[] {"{0}", "{0}"},
                new[] {"{0}", "{0}"},
                new[] {"{0}", "{0}"},
                new[] {"{0}", "{0}"},
                new[] {"{0}", "{0}"},
                new[] {"{0}", "{0}"},
                new[] {weeks},
                new[] {days},
                new[] {hours},
                new[] {minutes},
                new[] {seconds},
                new[] {"{0}"},
                lessThan
            );
            return Unit.Default;
        }
    }
}