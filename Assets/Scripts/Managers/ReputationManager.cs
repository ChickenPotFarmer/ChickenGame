using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    [Header("Settings")]
    public float minPricePerGram;
    public float maxPricePerGram;
    public float repForMaxPrice;

    [Header("Status")]
    public float rep;
    public float price;

    public static ReputationManager instance;
    [HideInInspector]
    public GameObject repManager;

    private void Awake()
    {
        instance = this;
        repManager = gameObject;
    }

    public void AddRep(float _amt)
    {
        rep += _amt;
    }

    public float GetPricePerGram()
    {
        // fraction of maxPrice
        float diff = maxPricePerGram - minPricePerGram;
        float frac;

        if (rep < repForMaxPrice)
            frac = rep / repForMaxPrice;
        else
            frac = 1;

        diff = diff * frac;

        price = minPricePerGram + diff;

        // random factor
        float rand = Random.Range(-3, 3);

        price += rand;

        ////round to .XX
        //price *= 100;
        price = Mathf.Round(price);
        //price /= 100;

        return price;
    }
}
