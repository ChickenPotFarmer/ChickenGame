using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrainProfile : MonoBehaviour
{
    [Header("Basic Info")]
    public string strainName;
    [Tooltip("Indica = 0,\nIndica Hybrid = 1,\nHybrid = 2,\nSativa Hybrid = 3,\nSativa = 4")]
    public int strainType;
    public float thcPercent;
    public string strainID;

    [Header("Terpenes Basic Info")]
    public float totalTerpenesPercent;
    public int primaryTerpene; //   55 - 85%
    public int secondaryTerpene; // 10 - 45%
    public int lesserTerpene; //    1 - 5%

    [Header("Terpene Levels")]
    [Tooltip("Terpene Int = 0")]
    public float caryophyllene; 
    [Tooltip("Terpene Int = 1")]
    public float limonene;
    [Tooltip("Terpene Int = 2")]
    public float linalool;
    [Tooltip("Terpene Int = 3")]
    public float myrcene;
    [Tooltip("Terpene Int = 4")]
    public float pinene;
    [Tooltip("Terpene Int = 5")]
    public float terpinolene;

    [Header("Terpene Effects / Flavors")]
    public string primaryEffect;
    public string secondaryEffect;

    [Header("Terpene Generator Settings")]
    public float primaryMin;
    public float primaryMax;
    public float secondaryMinRemaindar;
    public float secondaryMaxRemaindar;
    public float lesserMinRemaindar;
    public float lesserMaxRemaindar;

    private UniqueIdMaster uniqueIdMaster;

    private void Start()
    {
        if (!uniqueIdMaster)
            uniqueIdMaster = UniqueIdMaster.instance.uniqueIdMaster.GetComponent<UniqueIdMaster>();

        if (strainID == "")
            GenerateUniqueID();
    }

    public void SetStrain(StrainProfile _strain)
    {
        strainName = _strain.strainName;
        strainType = _strain.strainType;
        thcPercent = _strain.thcPercent;
        totalTerpenesPercent = _strain.totalTerpenesPercent;
        primaryTerpene = _strain.primaryTerpene;
        secondaryTerpene = _strain.secondaryTerpene;
        lesserTerpene = _strain.lesserTerpene;

        caryophyllene = _strain.caryophyllene;
        limonene = _strain.limonene;
        linalool = _strain.linalool;
        myrcene = _strain.myrcene;
        pinene = _strain.pinene;
        terpinolene = _strain.terpinolene;

        primaryEffect = _strain.primaryEffect;
        secondaryEffect = _strain.secondaryEffect;
    }

    public void GenerateUniqueID()
    {
        strainID = UniqueIdMaster.instance.uniqueIdMaster.GetComponent<UniqueIdMaster>().GetID();

        InventoryItem itemComp = GetComponent<InventoryItem>();

        if (itemComp != null)
            itemComp.itemID = strainID;
    }

    public void GenerateTerpeneEffects()
    {
        primaryEffect = TerpeneEffects.GetRandomEffect(primaryTerpene);
        do
        {
            secondaryEffect = TerpeneEffects.GetRandomEffect(secondaryTerpene);
        } while (primaryEffect == secondaryEffect);
    }

    public void GenerateTerpeneLevels()
    {
        ResetTerpeneLevels();

        primaryEffect = TerpeneEffects.GetRandomEffect(primaryTerpene);

        do
        {
            secondaryEffect = TerpeneEffects.GetRandomEffect(secondaryTerpene);
        } while (primaryEffect == secondaryEffect);

        float rand = Random.Range(primaryMin, primaryMax);
        SetTerpeneLevel(primaryTerpene, rand);

        float remainder = 1 - rand;
        rand = Random.Range(secondaryMinRemaindar, secondaryMaxRemaindar);

        float secondary = remainder * rand;
        SetTerpeneLevel(secondaryTerpene, secondary);

        remainder -= secondary;

        if (lesserTerpene != -1)
        {
            rand = Random.Range(lesserMinRemaindar, lesserMaxRemaindar);

            float lesser = remainder * rand;
            SetTerpeneLevel(lesserTerpene, lesser);
            remainder -= lesser;
        }

        SetOtherTerpenes(remainder);


    }

    public void SetTerpeneLevel(int _terpeneInt, float _amt)
    {
        switch (_terpeneInt)
        {
            case 0:
                caryophyllene = _amt;
                break;

            case 1:
                limonene = _amt;
                break;

            case 2:
                linalool = _amt;
                break;

            case 3:
                myrcene = _amt;
                break;

            case 4:
                pinene = _amt;
                break;

            case 5:
                terpinolene = _amt;
                break;
        }
    }

    public void SetOtherTerpenes(float _remainder)
    {
        float remainder = _remainder;
        float first;
        float second;
        float third;
        if (lesserTerpene != -1)
        {
            // First one
            float rand = Random.Range(0.25f, 0.4f);
            first = rand * remainder;
            if (caryophyllene == 0)
                caryophyllene = first;

            else if (limonene == 0)
                limonene = first;

            else if (linalool == 0)
                linalool = first;

            else if (myrcene == 0)
                myrcene = first;

            else if (pinene == 0)
                pinene = first;

            else if (terpinolene == 0)
                terpinolene = first;

            // Second one
            rand = Random.Range(0.25f, 0.4f);
            second = rand * remainder;

            if (caryophyllene == 0)
                caryophyllene = second;

            else if (limonene == 0)
                limonene = second;

            else if (linalool == 0)
                linalool = second;

            else if (myrcene == 0)
                myrcene = second;

            else if (pinene == 0)
                pinene = second;

            else if (terpinolene == 0)
                terpinolene = second;

            // third one
            remainder -= first + second;

            if (caryophyllene == 0)
                caryophyllene = remainder;

            else if (limonene == 0)
                limonene = remainder;

            else if (linalool == 0)
                linalool = remainder;

            else if (myrcene == 0)
                myrcene = remainder;

            else if (pinene == 0)
                pinene = remainder;

            else if (terpinolene == 0)
                terpinolene = remainder;
        }
        else
        {
            // First one
            float rand = Random.Range(0.15f, 0.35f);
            first = rand * remainder;
            if (caryophyllene == 0)
                caryophyllene = first;

            else if (limonene == 0)
                limonene = first;

            else if (linalool == 0)
                linalool = first;

            else if (myrcene == 0)
                myrcene = first;

            else if (pinene == 0)
                pinene = first;

            else if (terpinolene == 0)
                terpinolene = first;

            // Second one
            rand = Random.Range(0.15f, 0.35f);
            second = rand * remainder;

            if (caryophyllene == 0)
                caryophyllene = second;

            else if (limonene == 0)
                limonene = second;

            else if (linalool == 0)
                linalool = second;

            else if (myrcene == 0)
                myrcene = second;

            else if (pinene == 0)
                pinene = second;

            else if (terpinolene == 0)
                terpinolene = second;

            // Third one
            rand = Random.Range(0.15f, 0.35f);
            third = rand * remainder;

            if (caryophyllene == 0)
                caryophyllene = third;

            else if (limonene == 0)
                limonene = third;

            else if (linalool == 0)
                linalool = third;

            else if (myrcene == 0)
                myrcene = third;

            else if (pinene == 0)
                pinene = third;

            else if (terpinolene == 0)
                terpinolene = third;

            // fourth one
            remainder -= first + second + third;

            if (caryophyllene == 0)
                caryophyllene = remainder;

            else if (limonene == 0)
                limonene = remainder;

            else if (linalool == 0)
                linalool = remainder;

            else if (myrcene == 0)
                myrcene = remainder;

            else if (pinene == 0)
                pinene = remainder;

            else if (terpinolene == 0)
                terpinolene = remainder;
        }


        
    }

    public void ResetTerpeneLevels()
    {
        caryophyllene = 0;
        limonene = 0;
        linalool = 0;
        myrcene = 0;
        pinene = 0;
        terpinolene = 0;
    }

    public string GetStrainType()
    {
        string type;
        switch (strainType)
        {
            case 0:
                type = "Indica";
                break;

            case 1:
                type = "Indica Hybrid";
                break;

            case 2:
                type = "Hybrid";
                break;

            case 3:
                type = "Sativa Hybrid";
                break;

            case 4:
                type = "Sativa";
                break;


            default:
                type = "STRAIN TYPE ERROR";
                break;
        }

        return type;
    }

    public string GetReaderFriendlyThcContent()
    {
        float thc = thcPercent * 100;
        string readable = thc.ToString("n2") + "%";
        return readable;
    }

    public string GetPrimaryTerpene()
    {
        string terpene;

        switch (primaryTerpene)
        {
            case 0:
                terpene = "Caryophyllene";
                break;

            case 1:
                terpene = "Limonene";
                break;

            case 2:
                terpene = "Linalool";
                break;

            case 3:
                terpene = "Myrcene";
                break;

            case 4:
                terpene = "Pinene";
                break;

            case 5:
                terpene = "Terpinolene";
                break;

            default:
                terpene = "PRIMARY TERPENE ERROR";
                break;
        }

        return terpene;
    }

    public string GetSecondaryTerpene()
    {
        string terpene;

        switch (secondaryTerpene)
        {
            case 0:
                terpene = "Caryophyllene";
                break;

            case 1:
                terpene = "Limonene";
                break;

            case 2:
                terpene = "Linalool";
                break;

            case 3:
                terpene = "Myrcene";
                break;

            case 4:
                terpene = "Pinene";
                break;

            case 5:
                terpene = "Terpinolene";
                break;

            default:
                terpene = "SECONDARY TERPENE ERROR";
                break;
        }

        return terpene;
    }

    public string GetLesserTerpene()
    {
        string terpene;

        switch (lesserTerpene)
        {
            case 0:
                terpene = "Caryophyllene";
                break;

            case 1:
                terpene = "Limonene";
                break;

            case 2:
                terpene = "Linalool";
                break;

            case 3:
                terpene = "Myrcene";
                break;

            case 4:
                terpene = "Pinene";
                break;

            case 5:
                terpene = "Terpinolene";
                break;

            case -1:
                terpene = "None";
                break;

            default:
                terpene = "LESSER TERPENE ERROR";
                break;
        }

        return terpene;
    }

    public string GetReaderFriendlyTerpeneLvl(int _terpene)
    {
        float terpeneLvl;

        switch (_terpene)
        {
            case 0:
                terpeneLvl = caryophyllene;
                break;

            case 1:
                terpeneLvl = limonene;
                break;

            case 2:
                terpeneLvl = linalool;
                break;

            case 3:
                terpeneLvl = myrcene;
                break;

            case 4:
                terpeneLvl = pinene;
                break;

            case 5:
                terpeneLvl = terpinolene;
                break;

            default:
                terpeneLvl = -1;
                Debug.LogWarning("Terpene Levels Get Error");
                break;
        }

        string terpeneString = (terpeneLvl * 100).ToString("n2") + "%";

        return terpeneString;
    }

    public bool AddToTerpeneLvl(int _terepeneInt, float _amt)
    {
        bool success;
        switch (_terepeneInt)
        {
            case 0:
                if (caryophyllene + _amt <= 1 && caryophyllene - _amt > 0)
                {
                    success = true;
                    caryophyllene += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            case 1:
                if (limonene + _amt <= 1 && limonene - _amt > 0)
                {
                    success = true;
                    limonene += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            case 2:
                if (linalool + _amt <= 1 && linalool - _amt > 0)
                {
                    success = true;
                    linalool += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            case 3:
                if (myrcene + _amt <= 1 && myrcene - _amt > 0)
                {
                    success = true;
                    myrcene += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            case 4:
                if (pinene + _amt <= 1 && pinene - _amt > 0)
                {
                    success = true;
                    pinene += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            case 5:
                if (terpinolene + _amt <= 1 && terpinolene - _amt > 0)
                {
                    success = true;
                    terpinolene += _amt;
                }
                else
                {
                    success = false;
                }
                break;

            default:
                success = false;
                break;
        }

        return success;
    }

    public void BoostTerpene(int _terpeneBoosted, int _terpeneWeakened, float _amt)
    {
        if(AddToTerpeneLvl(_terpeneBoosted, _amt))
        {
            AddToTerpeneLvl(_terpeneWeakened, -_amt);

        }
        else
        {
            Debug.Log("Primary Terpene Boost Failed");
        }
    }
}
