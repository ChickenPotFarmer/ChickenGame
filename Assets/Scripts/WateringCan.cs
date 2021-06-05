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

    private void Awake()
    {
        instance = this;
        waterCan = gameObject;
    }

    private void Start()
    {
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
}
