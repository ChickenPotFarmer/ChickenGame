using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trimmer : MonoBehaviour
{
    [Header("Status")]
    public bool trimmerOn;
    public bool autoTrimOn;
    public WeedPlant selectedPlant;
    public TutorialPlant selectedTutorialPlant;


    [Header("Setup")]
    public GameObject trimmerModel;
    public GameObject trimmingsPrefab;
    public GameObject uiTrimmingsPrefab;

    [Header("Current Trimmer Settings")]
    public int minTrimmings;
    public int maxTrimmings;
    public float trimmerReach;

    private ChickenController chickenController;
    private InventoryController inventoryController;

    public static Trimmer instance;
    [HideInInspector]
    public GameObject trimmer;

    private void Awake()
    {
        instance = this;
        trimmer = gameObject;
    }

    private void Start()
    {
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (trimmerOn)
            trimmerModel.SetActive(true);
        else
            trimmerModel.SetActive(false);
    }

    //public void TrimPlant(WeedPlant _plant)
    //{
    //    int trimmings = Random.Range(minTrimmings, maxTrimmings + 1);

    //    if (inventoryController.CanTakeItem("00000001", trimmings))
    //    {
    //        selectedPlant = _plant;

    //        selectedPlant.trimmed = true;

    //        Xp.TrimPlant();

    //        StartCoroutine((SpawnRoutine(trimmings, selectedPlant.transform.position)));
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Inventory Full, cannont add trimmings");
    //    }
    //}

    IEnumerator SpawnRoutine(int _amt, Vector3 _pos)
    {
        GameObject trim;
        for (int i = 0; i < _amt; i++)
        {
            yield return new WaitForSeconds(0.12f);
            trim = Instantiate(trimmingsPrefab);
            trim.transform.position = _pos;
        }
    }

    public bool TrimmerIsOn()
    {
        bool on = false;

        if (trimmerOn || autoTrimOn)
            on = true;

        return on;
    }

    public void ToggleTrimmer()
    {
        if (trimmerOn)
        {
            trimmerOn = false;
            trimmerModel.SetActive(false);
        }
        else
        {
            trimmerOn = true;
            trimmerModel.SetActive(true);
        }
    }

    public void ToggleTrimmer(bool _on)
    {
        if (!_on)
        {
            trimmerOn = false;
            trimmerModel.SetActive(false);
        }
        else
        {
            trimmerOn = true;
            trimmerModel.SetActive(true);
        }
    }

    public void TargetForTrim(WeedPlant _plant)
    {
        selectedPlant = _plant;
        StartCoroutine(CheckChickenDistanceRoutine(false));
    }

    public void TargetForTrim(TutorialPlant _plant)
    {
        selectedTutorialPlant = _plant;
        StartCoroutine(CheckChickenDistanceRoutine(true));
    }

    IEnumerator CheckChickenDistanceRoutine(bool _tutorialOn)
    {
        float endCheck = Time.time + 5;
        GameObject newTrimmings;
        InventoryItem trimmingsItem;
        do
        {
            if (_tutorialOn)
            {
                if (Vector3.Distance(selectedTutorialPlant.transform.position, chickenController.transform.position) < 4 && !selectedTutorialPlant.trimmed && trimmerOn)
                {
                    int trimmings = Random.Range(minTrimmings, maxTrimmings + 1);

                    if (inventoryController.CanTakeItem("00000001", trimmings))
                    { 
                        selectedTutorialPlant.Trim();
                        chickenController.SetNewDestination(transform.position);
                        StartCoroutine((SpawnRoutine(trimmings, selectedTutorialPlant.transform.position)));
                        selectedTutorialPlant = null;

                        newTrimmings = Instantiate(uiTrimmingsPrefab);
                        trimmingsItem = newTrimmings.GetComponent<InventoryItem>();

                        trimmingsItem.SetAmount(trimmings);
                        inventoryController.ReturnToInventory(trimmingsItem);
                        


                        Xp.TrimPlant();
                    }
                    else
                    {
                        Alerts.DisplayMessage("Cannot trim plant, inventory full.");
                    }

                    break;
                }
            }
            else
            {
                if (Vector3.Distance(selectedPlant.transform.position, chickenController.transform.position) < 4 && !selectedPlant.trimmed && trimmerOn)
                {
                    int trimmings = Random.Range(minTrimmings, maxTrimmings + 1);

                    if (inventoryController.CanTakeItem("00000001", trimmings))
                    {

                        selectedPlant.Trim();
                        chickenController.SetNewDestination(transform.position);
                        StartCoroutine((SpawnRoutine(trimmings, selectedPlant.transform.position)));
                        selectedPlant = null;

                        newTrimmings = Instantiate(uiTrimmingsPrefab);
                        trimmingsItem = newTrimmings.GetComponent<InventoryItem>();

                        trimmingsItem.SetAmount(trimmings);
                        inventoryController.ReturnToInventory(trimmingsItem);
                        

                        Xp.TrimPlant();
                    }
                    else
                    {
                        Alerts.DisplayMessage("Cannot trim plant, inventory full.");
                    }

                    break;
                }
            }
            
            yield return new WaitForSeconds(0.2f);
        } while (Time.time < endCheck && selectedPlant != null);
        //selectedPlant = null;
    }
}
