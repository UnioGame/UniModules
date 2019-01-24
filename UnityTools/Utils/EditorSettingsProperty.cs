#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UniModule.UnityTools.Utils
{
    public class EditorSettingsProperty
    {
        public static bool GetBool(string key, ref bool? value, bool defaultValue = false)
        {

#if UNITY_EDITOR
            value = EditorPrefs.GetBool(key, defaultValue);
            return value.Value;
#else
            return value.HasValue ? value.Value : false;
#endif

        }

        public static void SetBool(string key, ref bool? value, bool newValue)
        {

#if UNITY_EDITOR
            EditorPrefs.SetBool(key, newValue);
#endif
            value = newValue;
        }
    }
}
