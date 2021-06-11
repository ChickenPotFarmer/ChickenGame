using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCrate : MonoBehaviour
{
    [Header("Status")]
    public bool placed;

    [Header("Setup")]
    public Transform slotsParent;
    public CanvasGroup cg;
    public Animator animator;

    private InventoryController inventoryController;
    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();
    }

    public void OpenCrate(bool _open)
    {
        if (_open)
        {
            StartCoroutine(OpenCrateRoutine(true));
        }
        else
        {
            StartCoroutine(OpenCrateRoutine(false));
        }
    }

    public void CloseCrate()
    {
        StartCoroutine(OpenCrateRoutine(false));
    }

    public IEnumerator OpenCrateRoutine(bool _open)
    {
        if (_open)
        {
            animator.Play("Open");
            yield return new WaitForSeconds(1);
            SetPanelActive(true);
        }
        else
        {
            SetPanelActive(false);
            yield return new WaitForSeconds(0.05f);
            animator.Play("Close");


        }
    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void PlaceCrate()
    {
        StartCoroutine(PlaceCrateRoutine());
    }

    IEnumerator PlaceCrateRoutine()
    {
        yield return new WaitForSeconds(1);
        placed = true;

    }

    public void ClearInventory()
    {
        inventoryController.ReturnAllItems(slotsParent);
    }
}
