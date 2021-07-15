using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySplat : MonoBehaviour
{
    public float lifespan;
    private List<PiggyPatrolController> piggies = new List<PiggyPatrolController>();

    private void Start()
    {
        Invoke(nameof(DestroySplat), lifespan);
    }

    private void DestroySplat()
    {
        for (int i = 0; i < piggies.Count; i++)
        {
            piggies[i].SetSticky(false);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.CompareTag("Piggy"))
            {
                PiggyPatrolController piggy = other.GetComponent<PiggyPatrolController>();
                piggy.SetSticky(true);
                piggies.Add(piggy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.CompareTag("Piggy"))
            {
                PiggyPatrolController piggy = other.GetComponent<PiggyPatrolController>();
                piggy.SetSticky(false);
                piggies.Remove(piggy);
            }
        }
    }
}
