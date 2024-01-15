using ITPRO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private float sensitivity;

    private bool _isActive;
    private Vector2 _screenSize;

    private void OnEnable()
    {
        _screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void LateUpdate()
    {
        if(!_isActive) return;
            
        Vector2 clampedPosition = Mouse.current.position.ReadValue() + JoystickInput.GetJoystick() * sensitivity;
        clampedPosition = new Vector2(
            Mathf.Clamp(clampedPosition.x, 0, _screenSize.x),
            Mathf.Clamp(clampedPosition.y, 0, _screenSize.y));

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.M))
        {
            Mouse.current.WarpCursorPosition(clampedPosition);
        }
#else
            Mouse.current.WarpCursorPosition(clampedPosition);
#endif            
    }
    
    public void MoveCursor() => _isActive = true;
}