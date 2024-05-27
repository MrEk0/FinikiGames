using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    
    [SerializeField]
    private float velocity;
    
    [SerializeField]
    [Min(0f)]
    private float joystickStartMovingForceCoefficient = 0.1f;
    
    private Rigidbody _rig;
    private Rigidbody Rig => _rig ??= GetComponent<Rigidbody>();

    private Vector3 _moveDirection = Vector3.zero;

    private bool _isMoving;

    private void Update()
    {
        if (joystick.InTouch && joystick.GetForce().sqrMagnitude > joystickStartMovingForceCoefficient)
        {
            _isMoving = true;
            
            var joystickForce = joystick.GetForce();
            _moveDirection = new Vector3(joystickForce.x, 0f, joystickForce.y).normalized;
        }
        else
        {
            _isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
            return;
        
        var t = transform;
        var tPos = t.position;
        
        tPos += _moveDirection * (velocity * Time.fixedDeltaTime);
        Rig.MovePosition(tPos);
    }
}
