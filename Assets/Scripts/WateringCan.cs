using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [Header("Status")]
    public bool waterCanOn;
    public float waterCanPower;

    [Header("Setup")]
    public GameObject waterCanModel;

    public static WateringCan instance;
    [HideInInspector]
    public GameObject waterCan;

    private WeedPlant selectedPlant;
    private ChickenController chickenController;

    private void Awake()
    {
        instance = this;
        waterCan = gameObject;
    }

    private void Start()
    {
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (waterCanOn)
            waterCanModel.SetActive(true);
        else
            waterCanModel.SetActive(false);
    }

    public void ToggleWaterCan()
    {
        if (waterCanOn)
        {
            waterCanOn = false;
            waterCanModel.SetActive(false);
        }
        else
        {
            waterCanOn = true;
            waterCanModel.SetActive(true);
        }
    }

    public void ToggleWaterCan(bool _on)
    {
        if (!_on)
        {
            waterCanOn = false;
            waterCanModel.SetActive(false);
        }
        else
        {
            waterCanOn = true;
            waterCanModel.SetActive(true);
        }
    }

    public void TargetForWater(WeedPlant _plant)
    {
        selectedPlant = _plant;
        StartCoroutine(CheckChickenDistanceRoutine());
    }

    IEnumerator CheckChickenDistanceRoutine()
    {
        float endCheck = Time.time + 5;
        do
        {
            if (Vector3.Distance(selectedPlant.transform.position, transform.position) < 4 && waterCanOn)
            {
                selectedPlant.Water();
                chickenController.SetNewDestination(transform.position);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        } while (Time.time < endCheck && selectedPlant != null);
        //selectedPlant = null;
    }
}
