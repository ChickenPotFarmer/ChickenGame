using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAlert : MonoBehaviour
{
    [SerializeField]
    private GameObject susAlert;

    [SerializeField]
    private GameObject pursuitAlert;

    public static ScreenAlert instance;
    [HideInInspector]
    public GameObject screenAlert;

    private void Awake()
    {
        instance = this;
        screenAlert = gameObject;
    }

    public void SetSus(bool _active)
    {
        if (_active)
        {
            if (!susAlert.activeInHierarchy && !pursuitAlert.activeInHierarchy)
                susAlert.SetActive(true);
        }
        else
        {
            susAlert.SetActive(false);
        }
    }

    public void SetPursuit(bool _active)
    {
        if (_active)
        {
            if (!pursuitAlert.activeInHierarchy)
            {
                pursuitAlert.SetActive(true);
                susAlert.SetActive(false);
            }
        }
        else
        {
            pursuitAlert.SetActive(false);
        }    
    }
}
