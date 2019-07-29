#if UNITY_IOS && UNITY_EDITOR
using UnityEditor.iOS.Xcode;

namespace PostprocessCollection
{
    /// <summary>
    /// Writes properties to Info.plist file of XCode project
    /// with given values.
    /// For example:
    /// ITSAppUsesNonExemptEncryption false - claims that your app doesn't use encryption
    /// GADApplicationIdentifier - ca-app-pub-3940256099942544~1458002511 - set Google Ads Plugin app id
    /// GADIsAdManagerApp true - another key, needed to make your app work with Google Ads Plugin
    /// </summary>
    public static class AddPropertiesPostprocess
    {
        public static void AddProperties(PlistDocument plist, PlistKeys keys)
        {            
            if(keys == null) return;

            if (keys.StringKeys != null)
            {
                foreach (var key in keys.StringKeys)
                    plist.root.SetString(key.Name, key.Value);
            }

            if (keys.IntKeys != null)
            {
                foreach (var key in keys.IntKeys)
                    plist.root.SetInteger(key.Name, key.Value);
            }

            if (keys.BoolKeys != null)
            {
                foreach (var key in keys.BoolKeys)
                    plist.root.SetBoolean(key.Name, key.Value);
            }

            if (keys.FloatKeys != null)
            {
                foreach (var key in keys.FloatKeys)
                    plist.root.SetReal(key.Name, key.Value);
            }

        }
       
    }
}
#endif