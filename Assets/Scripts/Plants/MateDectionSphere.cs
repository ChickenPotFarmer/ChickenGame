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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weed Plant"))
        {
            WeedPlant inRangePlant = other.GetComponent<WeedPlant>();

            if (inRangePlant != parentPlant)
                matesInRange.Add(other.GetComponent<WeedPlant>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weed Plant"))
        {
            matesInRange.Remove(other.GetComponent<WeedPlant>());
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
