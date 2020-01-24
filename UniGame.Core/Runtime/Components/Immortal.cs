using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.Components
{
    public class Immortal : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
