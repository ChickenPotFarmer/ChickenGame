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

    [Header("Terpene Generator Settings")]
    public float primaryMin;
    public float primaryMax;
    public float secondaryMinRemaindar;
    public float secondaryMaxRemaindar;
    public float lesserMinRemaindar;
    public float lesserMaxRemaindar;

    private void Update()
    {
        if (Input.GetKeyDown("b"))
            GenerateTerpeneLevels();
    }

    public void GenerateTerpeneLevels()
    {
        float rand = Random.Range(primaryMin, primaryMax);
        SetTerpeneLevel(primaryTerpene, rand);

        float remainder = 1 - rand;
        rand = Random.Range(secondaryMinRemaindar, secondaryMaxRemaindar);

        float secondary = remainder * rand;
        SetTerpeneLevel(secondaryTerpene, secondary);

        remainder -= secondary;
        rand = Random.Range(lesserMinRemaindar, lesserMaxRemaindar);

        float lesser = remainder * rand;

        if (lesserTerpene != -1)
        {
            SetTerpeneLevel(lesserTerpene, lesser);
        }


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
}
