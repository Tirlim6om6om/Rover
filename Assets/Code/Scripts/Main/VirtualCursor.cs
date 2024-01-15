using System;
using ITPRO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class VirtualCursor : MonoBehaviour
{

    [SerializeField] private float sens;
    [SerializeField] private RectTransform pos;
    [SerializeField] private Image viewCursor;
    [SerializeField] private InputSystemUIInputModule real;
    [SerializeField] private InputSystemUIInputModule virt;
    [SerializeField] private TextMeshProUGUI text;

    private Mouse m_VirtualMouse;
    private Mouse m_RealMouse;

    public static VirtualCursor instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    protected void Start()
    {
        if (InputSystem.GetDevice("VirtualMouse") == null)
        {
            m_VirtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        }
        else
        {
            m_VirtualMouse = (Mouse) InputSystem.GetDevice("VirtualMouse");
        }

        if (InputSystem.GetDevice("Mouse") != null)
        {
            m_RealMouse = (Mouse) InputSystem.GetDevice("Mouse");
        }
        else
        {
            if (InputSystem.GetDevice("Touchpad") != null)
            {
                m_RealMouse = (Mouse) InputSystem.GetDevice("Touchpad");
            }
        }
        
        SetCenter();
        
        JoystickInput.joy.UI.Point.performed += context => OutPos(context);
        //JoystickInput.joy.Controller.Axis.performed += context => JoyMove(context);
        //JoystickInput.joy.UI.Point.performed += context => MouseMove(context);
        JoystickInput.joy.UI.LeftMouse.performed += context => MouseDown(context);
        JoystickInput.joy.UI.LeftMouse.canceled += context => MouseDown(context);
        JoystickInput.joy.Controller.Trigger.performed += context => MouseDown(context);
        JoystickInput.joy.Controller.Trigger.canceled += context => MouseDown(context);
        //JoystickInput.joy.UI.PressLeftClickMouse.performed += context => PressMouse(context);
        UnityEngine.Cursor.visible = false;
    }

    public void SetCenter()
    {
        Vector2 newPos = new Vector2(Screen.width/2, Screen.height/2);
        m_VirtualMouse.WarpCursorPosition(newPos);
        InputState.Change(m_VirtualMouse.position, newPos);
        pos.anchoredPosition = m_VirtualMouse.position.ReadValue();
    }

    public void OutPos(InputAction.CallbackContext context)
    {
        //text.SetText(context.ReadValue<Vector2>().ToString());
        //print(context.ReadValue<Vector2>());
    }
    
    
    private void MouseDown(InputAction.CallbackContext context)
    {
        if (m_VirtualMouse == null)
            return;
        
        MouseButton? button = MouseButton.Left;

        if (button != null)
        {
            var isPressed = context.control.IsPressed();
            m_VirtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(button.Value, isPressed);

            InputState.Change(m_VirtualMouse, mouseState);
        }
    }


    private void FixedUpdate()
    {
        JoyMove(JoystickInput.joy.Controller.Axis.ReadValue<Vector2>());
        //MouseMove(JoystickInput.joy.UI.PosMouse.ReadValue<Vector2>());
        MouseMove();
        RealMouseDown();
    }

    public void JoyMove(Vector2 context)
    {
        Vector2 posMouse = m_VirtualMouse.position.ReadValue();
        Vector2 delta = context * sens;
        if (delta.x != 0 || delta.y != 0)
        {
            ActiveVirtual(true);
        }
        Vector2 newPos = posMouse + delta;
        newPos = new Vector2(Mathf.Clamp(newPos.x, 0, Screen.width), Mathf.Clamp(newPos.y, 0, Screen.height));
        m_VirtualMouse.WarpCursorPosition(newPos);
        InputState.Change(m_VirtualMouse.position, newPos);
        InputState.Change(m_VirtualMouse.delta, delta);
        pos.anchoredPosition = m_VirtualMouse.position.ReadValue();
    }
    
    public void MouseMove()
    {
        Vector2 delta = m_RealMouse.delta.ReadValue();
        if(delta.x == 0 && delta.y == 0) return;
        Vector2 newPos = m_VirtualMouse.position.ReadValue() + delta;
        newPos = new Vector2(Mathf.Clamp(newPos.x, 0, Screen.width), Mathf.Clamp(newPos.y, 0, Screen.height));
        m_VirtualMouse.WarpCursorPosition(newPos);
        InputState.Change(m_VirtualMouse.position, newPos);
        InputState.Change(m_VirtualMouse.delta, delta);

        Vector2 posVirtual = m_VirtualMouse.position.ReadValue();
        //InputState.Change(m_RealMouse.position, posVirtual);
        pos.anchoredPosition = posVirtual;
    }

    public void RealMouseDown()
    {
        if (m_VirtualMouse == null || m_RealMouse == null)
            return;

        MouseButton? button = MouseButton.Left;

        if (button != null)
        {
            var isPressed = m_RealMouse.press.isPressed;
            m_VirtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(button.Value, isPressed);
            InputState.Change(m_VirtualMouse, mouseState);
        }
    }

    private void ActiveVirtual(bool active)
    {
        return;
        viewCursor.enabled = active;
        real.enabled = !active;
        virt.enabled = active;
    }

    public void SetVisible(bool active)
    {
        viewCursor.enabled = active;
        if (active)
        {
            SetCenter();
        }
        else
        {
            
        }
    }
}