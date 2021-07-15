using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("Status")]
    public string currentItemSelected;

    [Header("Setup")]
    public CanvasGroup cg;
    public SeedCannon seedCannon;
    public Trimmer trimmer;
    public WateringCan waterCan;
    public SplatGun splatGun;


    public static RadialMenu instance;
    [HideInInspector]
    public GameObject radialMenu;

    private void Awake()
    {
        instance = this;
        radialMenu = gameObject;
    }

    public void SetMenuActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;

            ActivateTool(currentItemSelected);
        }
    }

    public void ActivateTool(string _toolTag)
    {
        TurnOffAllTools();

        switch (_toolTag)
        {
            case "CANNON":
                seedCannon.ToggleCannon(true);
                break;

            case "TRIMMER":
                trimmer.ToggleTrimmer(true);
                break;

            case "WATER CAN":
                waterCan.ToggleWaterCan(true);
                break;

            case "SPLAT CANNON":
                splatGun.ToggleCannon(true);
                break;

            case "NONE":
                TurnOffAllTools();
                break;

            case null:
                TurnOffAllTools();
                break;

            default:
                Debug.LogWarning("Error in radial menu. No Item Selected.");
                break;

        }
    }

    public void TurnOffAllTools()
    {
        trimmer.ToggleTrimmer(false);
        seedCannon.ToggleCannon(false);
        waterCan.ToggleWaterCan(false);
        splatGun.ToggleCannon(false);
    }
}
