using UnityEngine;
using UnityEngine.InputSystem;

public class Minimap : MonoBehaviour
{
    [SerializeField] private InputActionProperty activateAction;
    [SerializeField] private GameObject minimapPanel;

    private bool _isActive = true;


    private void Start()
    {
        activateAction.action.started += ActiveSwitch;
    }

    private void ActiveSwitch(InputAction.CallbackContext obj)
    {
        _isActive = !_isActive;
        minimapPanel.SetActive(_isActive);
    }
}