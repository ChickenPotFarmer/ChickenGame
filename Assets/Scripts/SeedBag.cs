using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
{
    [Header("Info")]
    public int seeds = 10;

    public void RemoveSeeds(int _amt)
    {
        seeds -= _amt;

        if (seeds <= 0)
            Destroy(gameObject);
    }
}
