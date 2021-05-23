using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedBrick : MonoBehaviour
{
    [Header("Contents")]
    public float grams;


    private StrainProfile strain;

    private void Awake()
    {
        if (!strain)
            strain = GetComponent<StrainProfile>();
    }
}
