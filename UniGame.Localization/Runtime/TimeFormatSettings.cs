namespace UniGame.Utils.Runtime
{
    using System;
    using UnityEngine;
    using UnityEngine.Localization;

    [Serializable]
    [CreateAssetMenu(menuName = "Taktika/Settings/Time Format Settings", fileName = nameof(TimeFormatSettings))]
    public class TimeFormatSettings : ScriptableObject
    {
        public LocalizedString Seconds;
        public LocalizedString Minutes;
        public LocalizedString Hours;
        public LocalizedString Days;
        public LocalizedString Weeks;
        public LocalizedString LessThan;
    }
}