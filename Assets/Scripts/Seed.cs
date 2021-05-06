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

    [Header("Manual Set Traits")]
    public bool manuallySetOnStart;

    public bool smooth;
    public bool purple;
    public bool tasty;
    public bool highYield;
    public bool highOilYield;
    public bool highKeefYield;

    public bool harsh;
    public bool lowYield;
    public bool highStemContent;
    //high seed content

    [Header("Dictionarys")]
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

        if(manuallySetOnStart)
        {
            manuallySetOnStart = false;
            ManuallySetStrain();
        }

    }

    //private void Update()
    //{
    //    // TESTING
    //    if (Input.GetKeyDown("x"))
    //        GenerateRandomStrain();
    //}

    public void GenerateNewTraits()
    {

        //ClearTraits();
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

    public void ManuallySetStrain()
    {
        // Good Traits
        if (smooth) goodTraits["Smooth"] = true;
        if (purple) goodTraits["Purple"] = true;
        if (tasty) goodTraits["Tasty"] = true;
        if (highYield) goodTraits["High Yield"] = true;
        if (highOilYield) goodTraits["High Oil Yield"] = true;
        if (highKeefYield) goodTraits["High Keef Yield"] = true;

        // Bad Traits
        if (harsh) goodTraits["Harsh"] = true;
        if (lowYield) goodTraits["Low Yield"] = true;
        if (highStemContent) goodTraits["High Stem Content"] = true;
    }

    public void GenerateGoodTraits(int _amt)
    {
        int rand;
        bool duplicates = false;

        for (int i = 0; i < _amt; i++)
        {
            //do // REMOVED DEPULCATES CHECK
            //{
                rand = Random.Range(0, goodTraitNames.Length);

                if (!goodTraits[goodTraitNames[rand]])
                {
                    goodTraits[goodTraitNames[rand]] = true;
                    duplicates = false;
                }
                else
                    duplicates = true;

            //} while (duplicates);
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
            //do // REMOVED DUPLICATE CHECK
            //{
                rand = Random.Range(0, badTraitNames.Length);

                if (!badTraits[badTraitNames[rand]])
                {
                    badTraits[badTraitNames[rand]] = true;
                    duplicates = false;
                }
                else
                    duplicates = true;

            //} while (duplicates);
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
