using UnityEngine;

public class Joystick : MonoBehaviour
{
    [Min(0)]
    [SerializeField]
    private int index;

    [Range(0.0001f, 1f)]
    [SerializeField]
    private float maxForce = .1f;

    [SerializeField] 
    private bool forceMouseTouch;

    public bool InTouch { get; private set; }

    public Vector2 StartPosition { get; set; } = Vector2.zero;
    public Vector2 CurrentPosition { get; private set; } = Vector2.zero;

    private bool _isActive = true;
    
#if UNITY_EDITOR
    private void Start()
    {
        forceMouseTouch = true;
    }
#endif

    private void Update()
    {
        if (!_isActive)
            return;

        if (!Input.touchSupported || forceMouseTouch)
        {
            if (Input.GetMouseButtonDown(index))
                StartPosition = Input.mousePosition;

            InTouch = Input.GetMouseButton(index);

            if (InTouch)
                CurrentPosition = Input.mousePosition;
        }
        else
        {
            if (!InTouch && Input.touchCount > index)
                StartPosition = Input.GetTouch(index).position;

            InTouch = Input.touchCount > index;

            if (InTouch)
                CurrentPosition = Input.GetTouch(index).position;
        }
    }

    public Vector2 GetForce()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        var force = Mathf.Max(0.0001f, maxForce * screenSize.magnitude);

        var offset = CurrentPosition - StartPosition;
        return Vector2.ClampMagnitude(offset / force, 1f);
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
        InTouch = false;
    }
}
