using UnityEngine;
using UnityEngine.Events;

public class TestingPanel : UserListPanel
{
    [Space] public UnityEvent<UserPanel> selectionUser = new ();

    protected override UserPanel SelectUser
    {
        get => selectUser;
        set
        {
            if (value != null)
            {
                selectionUser.Invoke(value);
            }
            
            selectUser = value;
        }
    }
}