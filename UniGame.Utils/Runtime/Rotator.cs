namespace UniGame.Utils
{
    using UnityEngine;

    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 direction = new Vector3(0, 0, 1f);
        [SerializeField] private float speed = 1f;
        
        private void Update () 
        {
            transform.Rotate(direction * (speed * Time.deltaTime * 100f));
        }
    }   
}
