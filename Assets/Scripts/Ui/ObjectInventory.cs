using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInventory : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CanvasGroup cg;
    public Transform inventoryParent;
    public bool inventoryActive;

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            inventoryActive = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            inventoryActive = false;
        }
    }
}
