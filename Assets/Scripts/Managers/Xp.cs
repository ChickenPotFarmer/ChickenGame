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

    public static int GetPlayerLevel()
    {
        return xpMaster.currentLvl;
    }

    public static void BuyComplete(float _amt)
    {
        xpMaster.BuyComplete(_amt);
    }

    public static void AddRep(float _amt)
    {
        xpMaster.AddRep(_amt);
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
