namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
	using UnityEngine;

	// This component will automatically reset a Rigidbody when it gets spawned/despawned
	[RequireComponent(typeof(Rigidbody))]
	public class PooledRigidbody : MonoBehaviour
	{
		protected virtual void OnSpawn()
		{
			// Do nothing
		}
		
		protected virtual void OnDespawn()
		{
			var rigidbody = GetComponent<Rigidbody>();
			
			// Reset velocities
			rigidbody.velocity        = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
}