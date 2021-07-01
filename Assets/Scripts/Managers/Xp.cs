using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Xp
{
    private static XpMaster xpMaster;

    public static void XpStart()
    {
        if (!xpMaster)
            xpMaster = XpMaster.instance.xpMaster.GetComponent<XpMaster>();
    }

    public static void AddXp(float _amt)
    {
        xpMaster.AddXp(_amt);
    }

    public static void PlantSeed()
    {
        xpMaster.PlantSeed();
    }

    public static void TrimPlant()
    {
        xpMaster.TrimPlant();
    }

    public static void WaterPlant()
    {
        xpMaster.WaterPlant();
    }

}
