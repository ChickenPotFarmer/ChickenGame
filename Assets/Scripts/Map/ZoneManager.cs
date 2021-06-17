using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("Status")]
    public bool zoneB;
    public bool zoneC;
    public bool zoneD;
    public bool zoneE;
    public bool zoneF;

    [Header("Setup")]
    public GameObject zoneBbarriers;
    public GameObject zoneCbarriers;
    public GameObject zoneDbarriers;
    public GameObject zoneEbarriers;
    public GameObject zoneFbarriers;


    private void Start()
    {
        SetBarriers();
    }

    private void SetBarriers()
    {
        if (zoneB)
            zoneBbarriers.SetActive(false);

        if (zoneC)
            zoneCbarriers.SetActive(false);

        if (zoneD)
            zoneDbarriers.SetActive(false);

        if (zoneE)
            zoneEbarriers.SetActive(false);

        if (zoneF)
            zoneFbarriers.SetActive(false);
    }
}
