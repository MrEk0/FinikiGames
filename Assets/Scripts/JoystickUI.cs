using UnityEngine;

public class JoystickUI : MonoBehaviour
{
    [SerializeField] 
    private Joystick joystick;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField] 
    private RectTransform circle;
    
    [SerializeField]
    private RectTransform outerCircle;
    
    [SerializeField]
    private RectTransform directionIndicatorTransform;

    [SerializeField] 
    private float maxLength = 150f;

    private Vector2 _startPosition = Vector2.zero;
    
    private void Start()
    {
        _startPosition = outerCircle.transform.position;
    }

    private void Update()
    {
        circle.transform.position = _startPosition;
        outerCircle.transform.position = _startPosition;

        if (!joystick.InTouch) 
            return;
        
        circle.transform.position = ScreenToCanvasPosition(canvas, joystick.CurrentPosition);
        outerCircle.transform.position = ScreenToCanvasPosition(canvas, joystick.StartPosition);

        var rotationAngle = Vector3.SignedAngle(Vector3.up,
            (joystick.CurrentPosition - joystick.StartPosition).normalized, Vector3.forward);
        directionIndicatorTransform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        var length = maxLength * canvas.localScale.magnitude;

        if ((joystick.CurrentPosition - joystick.StartPosition).magnitude > length)
        {
            var direction = joystick.CurrentPosition - joystick.StartPosition;

            var newDir = direction - Vector2.ClampMagnitude(direction, length);
            var position = new Vector2(joystick.StartPosition.x + newDir.x, joystick.StartPosition.y + newDir.y);

            outerCircle.transform.position = ScreenToCanvasPosition(canvas, position);

            joystick.StartPosition = position;
        }
    }

    private static Vector2 ScreenToCanvasPosition(RectTransform canvasRect, Vector2 screenPosition)
    {
        var rect = canvasRect.rect;
        var viewportPosition = new Vector2(screenPosition.x / rect.width, screenPosition.y / rect.height);
        return ViewportToCanvasPosition(canvasRect, viewportPosition);
    }

    private static Vector2 ViewportToCanvasPosition(RectTransform canvasRect, Vector2 viewportPosition)
    {
        var scale = canvasRect.sizeDelta;
        return Vector2.Scale(viewportPosition, scale);
    }
}
