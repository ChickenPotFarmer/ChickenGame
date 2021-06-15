using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreederMaster : MonoBehaviour
{
    [Header("Breed Settings")]
    public float primaryTerpeneBoostMax;
    public float maxThcBoost;
    public float maxTerpenesBoost;
    public float maxFemaleInfluence;    // percentage of the difference between the female plant's levels and the new plants levels.
                                        // this is added to the new plant levels


    // TO-DO: Check for matching top terpenes and add special boost to them
    // TO-DO: Contact jarett about 3d modesl for each growing stage



    [Header("Parents")]
    public StrainProfile male;
    public StrainProfile female;
    public StrainProfile newStrain;

    [Header("Terpenes")]
    public int[] terpeneInts;
    public float[] terpeneLvls;
    public int newPrimaryTerpene;
    public int newSecondaryTerpene;
    public int newLesserTerpene;

    private StrainInfoUI strainInfoUI;

    public static BreederMaster instance;
    [HideInInspector]
    public GameObject breederMaster;

    private void Awake()
    {
        instance = this;
        breederMaster = gameObject;
    }

    private void Start()
    {
        if (!strainInfoUI)
            strainInfoUI = StrainInfoUI.instance.strainInfoUI.GetComponent<StrainInfoUI>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown("b"))
    //        Breed();
    //}

    public StrainProfile Breed(WeedPlant _male, WeedPlant _female)
    {
        male = _male.currentStrain;
        female = _female.currentStrain;

        if (newStrain != null)
            newStrain = null;

        //Replace these with blank prefab with correct components 
        GameObject newStrainObj = new GameObject();
        newStrainObj.AddComponent<StrainProfile>();

        newStrain = newStrainObj.GetComponent<StrainProfile>();
        newStrain.GenerateUniqueID();

        // THC Percent
        newStrain.thcPercent = (male.thcPercent + female.thcPercent) / 2; // add random factor to increase / decrease
        newStrain.thcPercent += newStrain.thcPercent * (Random.Range(0, maxThcBoost));

        float strainTypeAvg = (male.strainType + female.strainType) / 2;
        newStrain.strainType = Mathf.RoundToInt(strainTypeAvg);
        newStrain.strainName = "New " + newStrain.GetStrainType() + " Strain";


        newStrain.totalTerpenesPercent = (male.totalTerpenesPercent + female.totalTerpenesPercent) / 2; // add random factor to increase / decrease
        newStrain.totalTerpenesPercent += newStrain.totalTerpenesPercent * (Random.Range(0, maxTerpenesBoost));

        //Average terpenes
        AverageTerpeneLevels();
        CalcTopTerpenes();

        //Primary terpene boost
        float rand = Random.Range(0.05f, primaryTerpeneBoostMax);
        newStrain.BoostTerpene(newStrain.primaryTerpene, newStrain.secondaryTerpene, rand);

        CalcTopTerpenes();

        newStrain.primaryTerpene = newPrimaryTerpene;
        newStrain.secondaryTerpene = newSecondaryTerpene;
        newStrain.lesserTerpene = newLesserTerpene;

        newStrain.GenerateTerpeneEffects(); // Make this more complicated. 1/3 chance for male side, 1/3 chance for female, 1/3 chance random.

        return newStrain;

        //strainInfoUI.SetStrainInfoActive(newStrain);
    }

    public void AverageTerpeneLevels()
    {
        float avg;
        avg = (male.caryophyllene + female.caryophyllene) / 2;
        newStrain.caryophyllene = avg;
        avg = (male.limonene + female.limonene) / 2;
        newStrain.limonene = avg;
        avg = (male.linalool + female.linalool) / 2;
        newStrain.linalool = avg;
        avg = (male.myrcene + female.myrcene) / 2;
        newStrain.myrcene = avg;
        avg = (male.pinene + female.pinene) / 2;
        newStrain.pinene = avg;
        avg = (male.terpinolene + female.terpinolene) / 2;
        newStrain.terpinolene = avg;


        // Add extra female influence
        float dif;
        dif = (female.caryophyllene - newStrain.caryophyllene) * (Random.Range(0, maxFemaleInfluence));
        newStrain.caryophyllene += dif;

        newStrain.limonene -= dif / 5;
        newStrain.linalool -= dif / 5;
        newStrain.myrcene -= dif / 5;
        newStrain.pinene -= dif / 5;
        newStrain.terpinolene -= dif / 5;

        dif = (female.limonene - newStrain.limonene) * (Random.Range(0, maxFemaleInfluence));
        newStrain.limonene += dif;

        newStrain.caryophyllene -= dif / 5;
        newStrain.linalool -= dif / 5;
        newStrain.myrcene -= dif / 5;
        newStrain.pinene -= dif / 5;
        newStrain.terpinolene -= dif / 5;

        dif = (female.linalool - newStrain.linalool) * (Random.Range(0, maxFemaleInfluence));
        newStrain.linalool += dif;

        newStrain.caryophyllene -= dif / 5;
        newStrain.limonene -= dif / 5;
        newStrain.myrcene -= dif / 5;
        newStrain.pinene -= dif / 5;
        newStrain.terpinolene -= dif / 5;

        dif = (female.myrcene - newStrain.myrcene) * (Random.Range(0, maxFemaleInfluence));
        newStrain.myrcene += dif;

        newStrain.caryophyllene -= dif / 5;
        newStrain.limonene -= dif / 5;
        newStrain.linalool -= dif / 5;
        newStrain.pinene -= dif / 5;
        newStrain.terpinolene -= dif / 5;

        dif = (female.pinene - newStrain.pinene) * (Random.Range(0, maxFemaleInfluence));
        newStrain.pinene += dif;

        newStrain.caryophyllene -= dif / 5;
        newStrain.limonene -= dif / 5;
        newStrain.linalool -= dif / 5;
        newStrain.myrcene -= dif / 5;
        newStrain.terpinolene -= dif / 5;

        dif = (female.terpinolene - newStrain.terpinolene) * (Random.Range(0, maxFemaleInfluence));
        newStrain.terpinolene += dif;

        newStrain.caryophyllene -= dif / 5;
        newStrain.limonene -= dif / 5;
        newStrain.linalool -= dif / 5;
        newStrain.myrcene -= dif / 5;
        newStrain.pinene -= dif / 5;
    }

    public void CalcTopTerpenes()
    {
        float tempf;
        int tempInt;
        int[] orderedIntArray = terpeneInts;
        SetTerpeneLvlsArray();
        for (int i = 0; i < terpeneLvls.Length - 1; i++)
        {
            if (terpeneLvls[i] < terpeneLvls[i +1])
            {
                tempf = terpeneLvls[i];
                terpeneLvls[i] = terpeneLvls[i + 1];
                terpeneLvls[i + 1] = tempf;
                tempInt = orderedIntArray[i];
                orderedIntArray[i] = orderedIntArray[i + 1];
                orderedIntArray[i + 1] = tempInt;
                i = -1;
            }
        }

        newPrimaryTerpene = orderedIntArray[0];
        newSecondaryTerpene = orderedIntArray[1];
        newLesserTerpene = orderedIntArray[2];
    }

    public void SetTerpeneLvlsArray()
    {
        terpeneLvls[0] = newStrain.caryophyllene;
        terpeneLvls[1] = newStrain.limonene;
        terpeneLvls[2] = newStrain.linalool;
        terpeneLvls[3] = newStrain.myrcene;
        terpeneLvls[4] = newStrain.pinene;
        terpeneLvls[5] = newStrain.terpinolene;
    }
}
