using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedBrick : MonoBehaviour
{
    [Header("Contents")]
    public bool isDry;
    public float grams;

    [Header("Setup")]
    public GameObject wetIndicator;


    private StrainProfile strain;

    private void Awake()
    {
        if (!strain)
            strain = GetComponent<StrainProfile>();
    }

    private void Start()
    {
        if (!isDry)
            wetIndicator.SetActive(true);
        else
            wetIndicator.SetActive(false);
    }

    public void SetDry(bool _setDry)
    {
        if (_setDry)
        {
            wetIndicator.SetActive(false);
            isDry = true;
        }
        else
        {
            wetIndicator.SetActive(true);
            isDry = false;
        }
    }
}
