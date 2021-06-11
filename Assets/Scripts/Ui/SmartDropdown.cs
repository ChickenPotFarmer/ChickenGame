using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartDropdown : MonoBehaviour
{
    [Header("Status")]
    public bool storageCrateOpen;

    [Header("Setup")]
    public GameObject dropDownBtn;
    public Transform parentDropDown;

    public static SmartDropdown instance;
    [HideInInspector]
    public GameObject smartDropdown;

    private void Awake()
    {
        instance = this;
        smartDropdown = gameObject;
    }


}
