using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Info")]
    public string strain = "New Weed Strain";
    public float potentency;
    public int type;

    [Header("Special Traits")]
    public int traitRating;

    public Dictionary<string, bool> goodTraits = new Dictionary<string, bool>();
    public string[] goodTraitNames;

    public Dictionary<string, bool> badTraits = new Dictionary<string, bool>();
    public string[] badTraitNames; 

    private void Start()
    {
        for (int i = 0; i < goodTraitNames.Length; i++)
        {
            goodTraits.Add(goodTraitNames[i], false);
        }

    }

    private void Update()
    {
        // TESTING
        if (Input.GetKeyDown("x"))
            GenerateRandomStrain();
    }

    public void GenerateRandomStrain()
    {

        ClearTraits();
        if (traitRating > 0)
        {
            GenerateGoodTraits(Mathf.Abs(traitRating));
        }
        else if (traitRating < 0)
        {
            GenerateBadTraits(Mathf.Abs(traitRating));

        }
        else if (traitRating == 0)
        {
            // no traits
        }
    }

    public void GenerateGoodTraits(int _amt)
    {
        int rand;
        bool duplicates = false;

        for (int i = 0; i < _amt; i++)
        {
            do
            {
                rand = Random.Range(0, goodTraitNames.Length);

                if (!goodTraits[goodTraitNames[rand]])
                {
                    goodTraits[goodTraitNames[rand]] = true;
                    duplicates = false;
                }
                else
                    duplicates = true;

            } while (duplicates);
        }

        print("Good traits: ");
        for (int i = 0; i < goodTraitNames.Length; i++)
        {
            if (goodTraits[goodTraitNames[i]])
            {
                print(goodTraitNames[i]);
            }
        }
    }
    public void GenerateBadTraits(int _amt)
    {
        int rand;
        bool duplicates = false;

        for (int i = 0; i < _amt; i++)
        {
            do
            {
                rand = Random.Range(0, badTraitNames.Length);

                if (!badTraits[badTraitNames[rand]])
                {
                    badTraits[badTraitNames[rand]] = true;
                    duplicates = false;
                }
                else
                    duplicates = true;

            } while (duplicates);
        }

        print("Bad traits: ");
        for (int i = 0; i < badTraitNames.Length; i++)
        {
            if (badTraits[badTraitNames[i]])
            {
                print(badTraitNames[i]);
            }
        }
    }

    public void ClearTraits()
    {
        for (int i = 0; i < goodTraitNames.Length; i++)
        {
            goodTraits[goodTraitNames[i]] = false;

        }

        for (int i = 0; i < badTraitNames.Length; i++)
        {
            badTraits[badTraitNames[i]] = false;

        }
    }
}
