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

    public void SetStorageDropdown(StorageCrate _storageContainer)
    {
        //spawn in dropdown buttons and name them

        // if in inventory already
            // if is strain
                // get strain info btn
            // transfer all

        // else if not in inventory already
    }

    private void ClearBtns()
    {
        
    }
}
