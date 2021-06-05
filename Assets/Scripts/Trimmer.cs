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

    public void TrimPlant(WeedPlant _plant)
    {
        int trimmings = Random.Range(minTrimmings, maxTrimmings + 1);

        if (inventoryController.CanTakeItem("00000001", trimmings))
        {
            selectedPlant = _plant;

            selectedPlant.trimmed = true;

            StartCoroutine((SpawnRoutine(trimmings, selectedPlant.transform.position)));
        }
        else
        {
            Debug.LogWarning("Inventory Full, cannont add trimmings");
        }
    }

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
        StartCoroutine(CheckChickenDistanceRoutine());
    }

    IEnumerator CheckChickenDistanceRoutine()
    {
        float endCheck = Time.time + 5;
        GameObject newTrimmings;
        InventoryItem trimmingsItem;
        do
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

                }
                else
                {
                    Debug.LogWarning("Inventory Full, cannont add trimmings");
                }

                break;
            }
            yield return new WaitForSeconds(0.2f);
        } while (Time.time < endCheck && selectedPlant != null);
        //selectedPlant = null;
    }
}
