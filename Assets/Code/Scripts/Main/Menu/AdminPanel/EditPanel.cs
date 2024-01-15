using System;
using System.Collections.Generic;
using ITPRO.Rover.User;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    private delegate bool Panel();
    private Panel _closePanel;

    public UnityEvent closePanel = new UnityEvent();
    
    [SerializeField] private List<EditField> parameterTexts;
    [SerializeField] private TMP_Text errorText;

    private bool _createUser;
    private User _user;

    private void OnEnable()
    {
        _closePanel = delegate()
        {
            foreach (EditField item in parameterTexts)
            {
                if (!item.SaveParameter()) return false;
            }

            return true;
        };
    }


    public void CreateUser()
    {
        if (!_closePanel())
        {
            errorText.text = "Не все поля заполнены";
            return;
        }

        if (_createUser)
        {
            GameUsers.CreateUser(parameterTexts[0].Value);
        }
        else
        {
            _user.Name = parameterTexts[0].Value;
            GameUsers.UpdateUser(_user);
        }

        closePanel.Invoke();
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        _createUser = true;
        gameObject.SetActive(true);
    }
    
    public void OpenPanel(User user)
    {
        _createUser = false;
        _user = user;
        
        parameterTexts[0].name = user.Name;
        parameterTexts[0].ParameterField.text = user.Name;

        gameObject.SetActive(true);
    }
}