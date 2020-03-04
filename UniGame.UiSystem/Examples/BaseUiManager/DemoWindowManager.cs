using UniGreenModules.UniGame.UiSystem.Runtime;
using UnityEngine;

namespace UniGreenModules.UniGame.UiSystem.Examples.BaseUiManager
{
    public enum DemoUiType
    {
        Element,
        Screen,
        Window,
    }
    
    public class DemoWindowManager : MonoBehaviour
    {
        public GameUiViewManager uiViewManager;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ShowDemoViewAsScreen(DemoUiType type)
        {
            switch (type) {
                case DemoUiType.Element:
                    uiViewManager.Open<DemoWindowView>(new ViewModelBase());
                    break;
                case DemoUiType.Screen:
                    uiViewManager.OpenScreen<DemoWindowView>(new ViewModelBase());
                    break;
                case DemoUiType.Window:
                    uiViewManager.OpenWindow<DemoWindowView>(new ViewModelBase());
                    break;
            }
        }
        

    }
}
