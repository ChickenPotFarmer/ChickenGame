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
        caryophyllene = 0;
        limonene = 0;
        linalool = 0;
        myrcene = 0;
        pinene = 0;
        terpinolene = 0;


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
}
