using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpMaster : MonoBehaviour
{
    [Header("Status")]
    public int currentLvl;
    public float xp;

    [Header("Plant XP Settings")]
    public float plantSeedXp;
    public float waterPlantXp;
    public float trimPlantXp;
    public float harvestPlantXp;

    [Header("Breeding XP Settings")]
    public float breedXp;
    public float newStrainBreedXp;

    [Header("Mission XP Settings")]
    public float xpPerGramDelivered;
    public float bankLoanPaidXp;

    [Header("Placeables XP Settings")]
    public float xpPerDollarSpent;

    [Header("Rep Settings")]
    public float repPerGramDelivered;

    [Header("XP Levels")]
    public float xpToNextLvl;
    public float[] xpToLevelUp;

    private ReputationManager repManager;

    public static XpMaster instance;
    [HideInInspector]
    public GameObject xpMaster;

    private void Awake()
    {
        instance = this;
        xpMaster = gameObject;
    }
    private void Start()
    {
        Xp.XpStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown("x"))
            Xp.AddXp(50);
    }

    public void BuyComplete(float _amt)
    {
        AddXp(xpPerGramDelivered * _amt);
        AddRep(repPerGramDelivered * _amt);
    }

    public void AddRep(float _amt)
    {
        if (!repManager)
            repManager = ReputationManager.instance.repManager.GetComponent<ReputationManager>();

        repManager.AddRep(_amt);

        Alerts.DisplayMessage(_amt + " rep gained!");
    }

    public void AddXp(float _amt)
    {
        xp += _amt;

        if (xp >= xpToLevelUp[currentLvl])
        {
            currentLvl++;
            //CalcXpToLvl();
        }

        Alerts.DisplayMessage("+" + _amt + " XP!");

    }

    //private void CalcXpToLvl()
    //{
    //    float xpDiff = 0.25f * (currentLvl + (300 * (2 * (currentLvl / 7)))); //does not work, look in to later
    //    xpToNextLvl += xpDiff;
    //}

    // Planting Seeds
    public void PlantSeed()
    {
        AddXp(plantSeedXp);
    }

    // Watering Plants
    public void WaterPlant()
    {
        AddXp(waterPlantXp);
    }

    // Trimming Plants
    public void TrimPlant()
    {
        AddXp(trimPlantXp);
    }

    // Harvesting Plants
    // Completing Orders
    // Buying Farm Placeables
    // Buying / Placing Auto Chicks
    // Breeding?



}
