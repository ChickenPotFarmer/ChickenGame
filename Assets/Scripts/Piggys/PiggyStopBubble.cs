using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyStopBubble : MonoBehaviour
{
    public float chanceToStop;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piggy"))
        {
            if (Random.value <= chanceToStop)
                other.gameObject.GetComponent<PiggyPatrolController>().RequestNewPatrol();
        }
    }
}
