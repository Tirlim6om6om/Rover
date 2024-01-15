using System;
using ITPRO.Rover.User;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    public UnityAction<UserPanel> deleteUser;
    public UnityAction<UserPanel> editUser;
    public Button selectButton;

    [SerializeField] private TMP_Text informationText;
    [SerializeField] private TMP_Text numText;

    public User PanelUser { get; set; }

    public void ChangeInformation(User user)
    {
        PanelUser = user;
        
        informationText.text = user.Name;
        numText.SetText(user.ID.ToString());
    }

    public void DeleteUser() => deleteUser.Invoke(this);
    public void EditUser() => editUser.Invoke(this);
} 