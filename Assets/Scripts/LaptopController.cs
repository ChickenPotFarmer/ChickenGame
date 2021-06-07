using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopController : MonoBehaviour
{
    [Header("Status")]
    public float inRangeDist;
    public bool chickenInRange;
    public bool clickActive;

    [Header("Setup")]
    public CanvasGroup laptopCg;

    private ChickenController chicken;
    private InventoryController inventoryController;

    public static LaptopController instance;
    [HideInInspector]
    public GameObject laptopController;

    private void Awake()
    {
        instance = this;
        laptopController = gameObject;
    }

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        StartCoroutine(InRangeCheck());
    }

    //private void Update()
    //{
    //    if (chickenInRange)
    //    {
    //        if (Input.GetMouseButtonDown(0) && clickActive)
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                if (hit.collider != null)
    //                {
    //                    if (hit.collider.gameObject.CompareTag("Laptop"))
    //                    {
    //                        SetLaptopPanelActive(true);
    //                        clickActive = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    IEnumerator InRangeCheck()
    {
        do
        {
            if (Vector3.Distance(transform.position, chicken.transform.position) < inRangeDist)
                chickenInRange = true;
            else
                chickenInRange = false;
            yield return new WaitForSeconds(0.2f);
        } while (true);
    }

    public void SetLaptopPanelActive(bool _active)
    {
        if (_active)
        {
            laptopCg.alpha = 1;
            laptopCg.interactable = true;
            laptopCg.blocksRaycasts = true;
        }
        else
        {
            laptopCg.alpha = 0;
            laptopCg.interactable = false;
            laptopCg.blocksRaycasts = false;
        }
    }

    public void CloseLaptop()
    {
        SetLaptopPanelActive(false);
        clickActive = true;
    }
}
