namespace UniGame.Shared.Runtime.MonoComponents
{
    using UnityEngine;

    public class ImmortalBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
