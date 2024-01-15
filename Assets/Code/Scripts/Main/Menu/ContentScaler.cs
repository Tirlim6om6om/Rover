using System;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.UI;

public class ContentScaler : MonoBehaviour
{
    [SerializeField] private RectTransform contentTransform;

    private void OnEnable()
    {
        GameUsers.createUser.AddListener(delegate { ChangeScale(); });
        GameUsers.deleteUser.AddListener(delegate { ChangeScale(); });
    }

    private void Start()
    {
        ChangeScale();
    }

    public void ChangeScale()
    {
        int childCount = contentTransform.childCount;

        contentTransform.sizeDelta = Vector2.up * (childCount * 110 + 59);
    }
}