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

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 2f);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Piggy") && !hit.isTrigger)
            {
                PiggyPatrolController piggy = hit.GetComponent<PiggyPatrolController>();
                piggy.SetSticky();
                piggies.Add(piggy);
                print("start worked");
            }
        }
    }

    private void DestroySplat()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.CompareTag("Piggy"))
            {
                PiggyPatrolController piggy = other.GetComponent<PiggyPatrolController>();
                piggy.SetSticky();
                //piggies.Add(piggy);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.isTrigger)
    //    {
    //        if (other.CompareTag("Piggy"))
    //        {
    //            PiggyPatrolController piggy = other.GetComponent<PiggyPatrolController>();
    //            piggy.SetSticky(false);
    //            piggies.Remove(piggy);
    //        }
    //    }
    //}
}
