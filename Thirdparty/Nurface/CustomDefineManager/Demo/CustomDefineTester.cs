using UnityEngine;
using System.Collections;

public class CustomDefineTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if VR_GEARVR
        Debug.Log("Gear VR");
#elif VR_GOOGLEVR
        Debug.Log("Google VR");
#endif
    }

    // Update is called once per frame
    void Update () {
	
	}
}
