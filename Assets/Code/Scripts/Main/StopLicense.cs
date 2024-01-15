using System;
using UnityEngine;

public class StopLicense : MonoBehaviour
{
    public GameObject menu;
    [SerializeField] private bool licesing;
    
    
    private void Awake()
    {
        if(!licesing) return;
        int day = int.Parse(System.DateTime.Now.ToString("dd"));
        int month = int.Parse(System.DateTime.Now.ToString("MM"));
        int year = int.Parse(System.DateTime.Now.ToString("yyyy"));
        print(day + " : " +month + " : " + year);
        if ((year >= 2023 && month > 7) || (year >= 2023 && month == 7 && day > 20))
        {
            Destroy(menu);
        }
    }
}