namespace UniGame.Utils.Runtime
{
    using System;
    using UnityEngine.Localization.SmartFormat.Utilities;

    public static class LocalizationHelper
    {
        private static TimeTextInfo TimeTextInfo = CommonLanguagesTimeTextInfo.English;
        public static event Action TimeFormatUpdated = delegate { };
        
        public static TimeTextInfo LocalizedTimeTextInfo
        {
            get => TimeTextInfo;
            set
            {
                TimeTextInfo = value;
                TimeFormatUpdated.Invoke();
            }
        }
    }
}