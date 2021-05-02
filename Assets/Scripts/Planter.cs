using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [Header("Seeds")]
    public int seeds;

    [Header("Selected Plant")]
    public WeedPlant selectedPlant;
    public WeedPlant activePlant;

    [Header("Settings")]
    public LayerMask weedLayer;
    public bool planterOn;
    public CanvasGroup newPlantPanelCg;

    [Header("Setup")]
    public GameObject detectorSphere;
    public PlanterSphere planterSphere;

    public static Planter instance;
    [HideInInspector]
    public GameObject planter;

    private void Awake()
    {
        instance = this;
        planter = gameObject;
    }

    //private void Update()
    //{
    //    if (planterOn && seeds != 0)
    //    {
    //        planterSphere.MoveSphere();

    //        if (selectedPlant != null)
    //        {
    //            if (Input.GetMouseButtonDown(0) && !selectedPlant.selected)
    //            {
                    
    //                selectedPlant.selected = true;
    //                SetNewPlantPanelActive(true);
    //                planterOn = false;
                    
    //            }
    //        }

    //    }

    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    //    RaycastHit hit;

    //    //    if (Physics.Raycast(ray, out hit))
    //    //    {
    //    //        if (hit.collider != null)
    //    //        {
    //    //            if (hit.collider.gameObject.CompareTag("Weed Plant"))
    //    //            {
    //    //                WeedPlant weedPlant = hit.collider.gameObject.GetComponent<WeedPlant>();
    //    //                if (weedPlant.fullyGrown)
    //    //                {
    //    //                    weedPlant.Harvest();
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}

    public void SetNewPlantPanelActive(bool _active)
    {
        if (seeds != 0)
        {
            if (_active)
            {
                activePlant = selectedPlant;
                newPlantPanelCg.alpha = 1;
                newPlantPanelCg.interactable = true;
                newPlantPanelCg.blocksRaycasts = true;
            }
            else
            {
                activePlant = null;
                newPlantPanelCg.alpha = 0;
                newPlantPanelCg.interactable = false;
                newPlantPanelCg.blocksRaycasts = false;
            }
        }
    }

    public void CancelNewPlantPanel()
    {
        activePlant.selected = false;
        SetNewPlantPanelActive(false);
        planterOn = true;
        
    }

    public void PlantNewMalePlant()
    {
        try
        { 
            activePlant.isPlanted = true;
            activePlant.isFemale = false;
            activePlant.selected = false;
            activePlant.Plant();
            SetNewPlantPanelActive(false);
            seeds--;
            planterOn = true;
        }
        catch
        {
            Debug.LogWarning("Error while planting new plant.");
        }
    }

    public void PlantNewFemalePlant()
    {
        try
        {
            if (activePlant != null)
            {
                activePlant.isPlanted = true;
                activePlant.isFemale = true;
                activePlant.selected = false;
                activePlant.Plant();
                SetNewPlantPanelActive(false);
                seeds--;
                planterOn = true;
            }
            else
                Debug.LogWarning("Error while planting new plant. Selected plant is null in Planter script.");

        }
        catch
        {
            Debug.LogWarning("Error while planting new plant.");
        }
    }

}
