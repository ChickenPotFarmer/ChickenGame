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

    [Header("XP Levels")]
    public float[] xpToLevelUp;


    public void PlantSeed()
    {

    }

    // Planting Seeds
    // Watering Plants
    // Trimming Plants
    // Harvesting Plants
    // Completing Orders
    // Buying Farm Placeables
    // Buying / Placing Auto Chicks
    // Breeding?



}
