using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCannon : MonoBehaviour
{
    [Header("Strain")]
    public StrainProfile currentStrain;

    [Header("Status")]
    public bool cannonOn;

    [Header("Setup")]
    public Transform debugTarget;
    public Transform firePoint;
    public GameObject seedProjectile;
    public GameObject cannonModel;

    public static SeedCannon instance;
    [HideInInspector]
    public GameObject seedCannon;

    private void Awake()
    {
        instance = this;
        seedCannon = gameObject;
    }

    private void Start()
    {
        if (cannonOn)
            cannonModel.SetActive(true);
        else
            cannonModel.SetActive(false);
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //        FireCannon(debugTarget);
    //}


    public void FireCannon(Transform _target)
    {
        GameObject newSeed = Instantiate(seedProjectile);
        newSeed.transform.position = firePoint.position;
        Seed seedComp = newSeed.GetComponent<Seed>();

        seedComp.target = _target;

        WeedPlant weedPlant = _target.GetComponent<WeedPlant>();

        if (weedPlant)
            weedPlant.hasSeed = true;

        if (currentStrain != null)
            newSeed.GetComponent<Seed>().currentStrain = currentStrain;
    }

    public void ToggleCannon()
    {
        if (cannonOn)
        {
            cannonOn = false;
            cannonModel.SetActive(false);
        }
        else
        {
            cannonOn = true;
            cannonModel.SetActive(true);
        }
    }
}
