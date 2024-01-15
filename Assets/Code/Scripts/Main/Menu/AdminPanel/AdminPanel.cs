using System;
using System.Collections.Generic;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : UserListPanel
{
    [SerializeField] private EditPanel editPanel;

    protected override void Awake()
    {
        base.Awake();
        GameUsers.createUser.AddListener(ShowUser);
        GameUsers.editUser.AddListener(EditUser);
    }

    private void DeleteUser(UserPanel userPanel)
    {
        if(userPanel == null) return;
        
        GameUsers.DeleteUser(userPanel.PanelUser);
        Destroy(userPanel.gameObject);
    }

    private void OpenEditPanel(UserPanel userPanel)
    {
        if(userPanel == null) return;

        editPanel.OpenPanel(userPanel.PanelUser);
        gameObject.SetActive(false);
    }

    public void CreateUser()
    {
        editPanel.OpenPanel();
        gameObject.SetActive(false);
    }

    private void EditUser(User user)
    {
        foreach (UserPanel userPanel in userPanels)
        {
            if (userPanel.PanelUser.ID == user.ID)
            {
                userPanel.ChangeInformation(user);
            }
        }
    }

    protected override void ShowUser(User user)
    {
        userPanels ??= new List<UserPanel>();
        
        UserPanel userPanel = Instantiate(userPanelPrefab, Vector3.zero, Quaternion.identity, userContent)
            .GetComponent<UserPanel>();
                
        userPanels.Add(userPanel);
        userPanel.ChangeInformation(user);
        userPanel.selectButton.onClick.AddListener(delegate { SelectUser = userPanel; });
        userPanel.deleteUser += DeleteUser;
        userPanel.editUser += OpenEditPanel;
    }
}