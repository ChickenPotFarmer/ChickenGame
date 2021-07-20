using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDetector : MonoBehaviour
{
    public List<WeedPlant> plantList = new List<WeedPlant>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weed Plant"))
        {
            WeedPlant plant = other.GetComponent<WeedPlant>();
            if (!plantList.Contains(plant))
                plantList.Add(plant);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weed Plant"))
        {
            WeedPlant plant = other.GetComponent<WeedPlant>();
            if (plantList.Contains(plant))
                plantList.Remove(plant);
        }
    }
}
