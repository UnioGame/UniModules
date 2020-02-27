using UnityEngine;

namespace GBG.SafeArea
{
    using Runtime;
    using UnityEditor;

    public class SafeAreaCommands : MonoBehaviour
    {
        [MenuItem(itemName:"iPhoneX",menuItem = "Tools/SafeArea/iPhoneX")]
        public static void EmulateSafeAreaIPhoneX()
        {
            SafeAreaConstants.Sim = SimDevice.iPhoneX;
        }
        
        [MenuItem(itemName:"iPhoneXsMax",menuItem = "Tools/SafeArea/iPhoneXsMax")]
        public static void EmulateSafeAreaiIPhoneXsMax()
        {
            SafeAreaConstants.Sim = SimDevice.iPhoneXsMax;
        }
        
        [MenuItem(itemName:"None",menuItem = "Tools/SafeArea/None")]
        public static void EmulateSafeAreaNone()
        {
            SafeAreaConstants.Sim = SimDevice.None;
        }
    }
}
