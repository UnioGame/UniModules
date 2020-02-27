namespace GBG.SafeArea.Runtime
{
    using UnityEngine;

    /// <summary>
    /// Safe area implementation for notched mobile devices. Usage:
    ///  (1) Add this component to the top level of any GUI panel. 
    ///  (2) If the panel uses a full screen background image, then create an immediate child and put the component on that instead, with all other elements childed below it.
    ///      This will allow the background image to stretch to the full extents of the screen behind the notch, which looks nicer.
    ///  (3) For other cases that use a mixture of full horizontal and vertical background stripes, use the Conform X & Y controls on separate elements as needed.
    /// </summary>
    public class SafeArea : MonoBehaviour
    {

        RectTransform         Panel;
        Rect                  LastSafeArea = new Rect(0, 0, 0, 0);
        
        [SerializeField] bool ConformLeft     = true; // Conform to screen safe area on X-axis (default true, disable to ignore)
        [SerializeField] bool ConformRight = true;
        
        [SerializeField] bool ConformTop     = true; // Conform to screen safe area on Y-axis (default true, disable to ignore)
        [SerializeField] bool ConformBottom= true;
        
        private void Awake()
        {
            Panel = GetComponent<RectTransform>();

            if (Panel == null) {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
            }

            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            var safeArea = GetSafeArea();

            if (safeArea != LastSafeArea)
                ApplySafeArea(safeArea);
        }

        private Rect GetSafeArea()
        {
            var safeArea = Screen.safeArea;

            if (!Application.isEditor || SafeAreaConstants.Sim == SimDevice.None) {
                return safeArea;
            }

            var nsa = new Rect(0, 0, Screen.width, Screen.height);

            switch (SafeAreaConstants.Sim) {
                case SimDevice.iPhoneX:
                    nsa = Screen.height > Screen.width ? 
                        SafeAreaConstants.NSA_iPhoneX[0] : 
                        SafeAreaConstants.NSA_iPhoneX[1];
                    break;
                case SimDevice.iPhoneXsMax:
                    nsa = Screen.height > Screen.width ? 
                        SafeAreaConstants.NSA_iPhoneXsMax[0] : 
                        SafeAreaConstants.NSA_iPhoneXsMax[1];
                    break;
                default:
                    break;
            }

            safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);

            return safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;

            // Ignore x-axis?
            if (!ConformLeft) {
                var delta = r.x;
                r.x     = 0;
                r.width = Mathf.Min(delta + r.width,Screen.width);
            }

            if (!ConformRight) {
                var delta = Screen.width - r.x - r.width;
                r.width = Mathf.Min(delta + r.width,Screen.width);
            }
            
            // Ignore y-axis?
            if (!ConformBottom)
            {
                var dif = r.y;
                r.y      =  0;
                r.height = Mathf.Min(dif + r.height,Screen.height);
            }
                       
            if (!ConformTop)
            {
                var dif = Screen.height - r.y - r.height;
                r.height = Mathf.Min(dif + r.height,Screen.height);
            }

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;
            anchorMin.x     /= Screen.width;
            anchorMin.y     /= Screen.height;
            anchorMax.x     /= Screen.width;
            anchorMax.y     /= Screen.height;
            Panel.anchorMin =  anchorMin;
            Panel.anchorMax =  anchorMax;

        }
    }
}