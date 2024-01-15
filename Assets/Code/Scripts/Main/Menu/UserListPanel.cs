using System;
using System.Collections.Generic;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserListPanel : MonoBehaviour
{
    public UnityEvent openPanel = new();

    [SerializeField] protected GameObject userPanelPrefab;
    [SerializeField] protected Transform userContent;
    
    private protected UserPanel selectUser;
    protected List<UserPanel> userPanels = new List<UserPanel>();

    protected virtual UserPanel SelectUser
    {
        get => selectUser;
        set => selectUser = value;
    }

    protected virtual void Awake()
    {
        GameUsers.GetFolderUsers();
        ShowUsers();
    }

    private void OnEnable()
    {
        openPanel.Invoke();
    }

    public void ShowUsers()
    {
        userPanels ??= new List<UserPanel>();
        
        for (int i = 0; i < GameUsers.Users.Count; i++)
        {
            if (i < userPanels.Count)
                userPanels[i].ChangeInformation(GameUsers.Users[i]);
            else
                ShowUser(GameUsers.Users[i]);
        }

        if (userPanels == null) return;
        for (int i = GameUsers.Users.Count; i < userPanels.Count; i++)
        {
            if(userPanels[i]) Destroy(userPanels[i].gameObject);
        }
    }
    
    protected virtual void ShowUser(User user)
    {
        userPanels ??= new List<UserPanel>();
        
        UserPanel userPanel = Instantiate(userPanelPrefab, Vector3.zero, Quaternion.identity, userContent)
            .GetComponent<UserPanel>();
                
        userPanels.Add(userPanel);
        userPanel.ChangeInformation(user);
        userPanel.selectButton.onClick.AddListener(delegate { SelectUser = userPanel; });
    }
}