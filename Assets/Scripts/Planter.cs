using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [Header("Seeds")]
    public int seeds;

    [Header("Selected Plant")]
    public WeedPlant selectedPlant;

    [Header("Settings")]
    public LayerMask weedLayer;
    public bool planterOn;
    public CanvasGroup newPlantPanelCg;

    [Header("Setup")]
    public GameObject detectorSphere;
    public PlanterSphere planterSphere;

    private void Update()
    {
        if (planterOn && seeds != 0)
        {
            planterSphere.MoveSphere();

            if (selectedPlant != null)
            {
                if (Input.GetMouseButtonDown(0) && !selectedPlant.selected)
                {
                    
                    selectedPlant.selected = true;
                    SetNewPlantPanelActive(true);
                    planterOn = false;
                    
                }
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Weed Plant"))
                    {
                        WeedPlant weedPlant = hit.collider.gameObject.GetComponent<WeedPlant>();
                        if (weedPlant.fullyGrown)
                        {
                            weedPlant.Harvest();
                        }
                    }
                }
            }
        }
    }

    private void SetNewPlantPanelActive(bool _active)
    {
        if (seeds != 0)
        {
            if (_active)
            {
                newPlantPanelCg.alpha = 1;
                newPlantPanelCg.interactable = true;
                newPlantPanelCg.blocksRaycasts = true;
            }
            else
            {
                newPlantPanelCg.alpha = 0;
                newPlantPanelCg.interactable = false;
                newPlantPanelCg.blocksRaycasts = false;
            }
        }
    }

    public void CancelNewPlantPanel()
    {
        SetNewPlantPanelActive(false);
        planterOn = true;
    }

    public void PlantNewMalePlant()
    {
        try
        { 
            selectedPlant.isPlanted = true;
            selectedPlant.isFemale = false;
            selectedPlant.selected = false;
            selectedPlant.Plant();
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
            selectedPlant.isPlanted = true;
            selectedPlant.isFemale = true;
            selectedPlant.selected = false;
            selectedPlant.Plant();
            SetNewPlantPanelActive(false);
            seeds--;
            planterOn = true;
        }
        catch
        {
            Debug.LogWarning("Error while planting new plant.");
        }
    }

}
