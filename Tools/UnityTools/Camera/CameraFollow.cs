using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Vector3 Offset;
	public Transform Target;

	private void  Update ()
	{
		transform.position = Target.position + Offset;
	}

}