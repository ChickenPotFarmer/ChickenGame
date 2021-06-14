using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateDectionSphere : MonoBehaviour
{
    public List<WeedPlant> matesInRange;
    public WeedPlant parentPlant;

    private void Start()
    {
        if (!parentPlant)
            parentPlant = GetComponentInParent<WeedPlant>();

        DetectMates();
    }
    
    public void BustinMakesMeFeelGood()
    {

        for (int i = 0; i < matesInRange.Count; i++)
        {
            if (matesInRange[i].isPlanted && !matesInRange[i].isMale)
            {
                matesInRange[i].PollinatePlant();
            }
        }
        
    }

    public void DetectMates()
    {
        matesInRange.Clear();
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Weed Plant"))
            {
                WeedPlant inRangePlant = hit.GetComponent<WeedPlant>();

                if (inRangePlant != parentPlant)
                    matesInRange.Add(inRangePlant);
            }


        }
    }
}
