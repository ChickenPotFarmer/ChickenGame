using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedPriceMaster : MonoBehaviour
{
    public float currentPrice;

    public static WeedPriceMaster instance;
    [HideInInspector]
    public GameObject weedPriceMaster;

    private void Awake()
    {
        instance = this;
        weedPriceMaster = gameObject;
    }

}
