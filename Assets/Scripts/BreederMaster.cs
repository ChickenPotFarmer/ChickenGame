using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreederMaster : MonoBehaviour
{
    [Header("Parents")]
    public Seed maleParent;
    public Seed femaleParent;

    [Header("Breeding Info")]
    public int avgTraitRating;
    public int newTraitRating;
    public int newBadTraits; // does not check for duplicates to add small difficult decrease
    public float traitRateBoost1Chance;
    public float traitRateBoost2Chance;
    public float traitRateBoost3Chance;

    public float badTrait1Chance;
    public float badTrait2Chance;

    private void Update()
    {
        // TESTING
        if (Input.GetKeyDown("x"))
            CalcNewTraitRating();
    }


    public void CalcNewTraitRating()
    {
        float avg = (maleParent.traitRating + femaleParent.traitRating) / 2;
        avgTraitRating = Mathf.RoundToInt(avg);

        newTraitRating = avgTraitRating;

        float rand = Random.Range(0.0f, 1.0f);

        if (rand < traitRateBoost1Chance)
        {
            newTraitRating++;
        }

        rand = Random.Range(0.0f, 1.0f);

        if (rand < traitRateBoost2Chance)
        {
            newTraitRating++;
        }

        rand = Random.Range(0.0f, 1.0f);

        if (rand < traitRateBoost3Chance)
        {
            newTraitRating++;
        }

        rand = Random.Range(0.0f, 1.0f);

        if (rand < badTrait1Chance)
        {
            newBadTraits++;
        }

        rand = Random.Range(0.0f, 1.0f);

        if (rand < badTrait2Chance)
        {
            newBadTraits++;
        }
    }
}
