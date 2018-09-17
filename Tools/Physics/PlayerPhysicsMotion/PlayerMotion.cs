using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MotionData {
    public float Angle;
    public Vector3 Force;

}

public class PlayerMotion : MonoBehaviour {

    private Rigidbody _rigidbody;
    
    private bool _isPrepared;

    [SerializeField]
    public MotionData Data;
    [SerializeField]
    private int _pointsCount = 20;
    [SerializeField]
    private float _dotSeparation = 0.2f;
    [SerializeField]
    private float _dotShift = 0.2f;
    [SerializeField]
    private Vector3 _force = new Vector3(10,10,0);
    [SerializeField]
    private Transform _worldPosition;
    [SerializeField]
    private ForceMode _forceMode = ForceMode.Impulse;
    
    public List<Vector3> Trajectory = new List<Vector3>();
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        Data = new MotionData();
    }

    private void Update() {

        if (Input.GetMouseButtonUp(0)) {
            if (_isPrepared) {
                ApplyForce();
            }
            _isPrepared = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            _isPrepared = true;
        }

        if (_isPrepared) {
            
            var position = Input.mousePosition;
            position.z = 10;
        
            var worldPosition = Camera.main.ScreenToWorldPoint(position);
            UpdateTrajectory(_pointsCount,worldPosition);
        }
    }

    private void ApplyForce() {
        _rigidbody.AddForce(Data.Force,_forceMode);
    }
    
    private void UpdateTrajectory(int pointCount, Vector3 worldPosition) {
        
        Trajectory.Clear();
        
        var initialiPoint = transform.position;
        _worldPosition.position = worldPosition;

        var isLeftSideX = transform.up.
            IsLeft(transform.InverseTransformPoint(worldPosition));
        var isLeftSideY = transform.right.
            IsLeft(transform.InverseTransformPoint(worldPosition));

        var forceDirection = worldPosition - initialiPoint;
        var normalizedPosition = forceDirection.normalized;
        
        Data.Force = Vector3.Scale(normalizedPosition, _force);
//        Data.Angle = Vector3.Angle(Vector3.right, Data.Force);
//        Data.Angle = isLeftSideX ? 180 - Data.Angle : Data.Angle ;
//                                
//        var rad = Mathf.Deg2Rad * Data.Angle;
//
//        var cosX = Mathf.Cos(rad);
//        var sinY = Mathf.Sin(rad);
        var deltaTime = Time.fixedDeltaTime;
        var deltaTimeSqr = deltaTime * deltaTime;
        var currentTime = 0f;
        var halfGravityY = 0.5f * -Physics.gravity.y;

        for (int i = 0; i < pointCount; i++) {

            var shift = _dotSeparation * i + _dotShift;
            
            var a = (halfGravityY * deltaTimeSqr * shift * shift);
            var x = Data.Force.x * deltaTime  * shift;
            var y = (Data.Force.y * deltaTime * shift - a);

            Trajectory.Add(initialiPoint + new Vector3(x,y));
        }
        
    }
    
	
}
