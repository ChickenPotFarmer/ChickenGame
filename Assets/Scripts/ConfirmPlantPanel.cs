using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPlantPanel : MonoBehaviour
{
    [Header("Setup")]
    public CanvasGroup cg;
    public Text confirmTxt;
    public bool active;
    public StrainProfile potentialStrain;
    public WeedPlant potentialPlant;
    public SeedBag seedBag;

    public static ConfirmPlantPanel instance;
    [HideInInspector]
    public GameObject confirmPlantPanel;

    private void Awake()
    {
        instance = this;
        confirmPlantPanel = gameObject;

        potentialStrain = GetComponent<StrainProfile>();
    }

    public void SetConfirmPanelActive(bool _active)
    {
        if (_active)
        {
            active = true;
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;

            confirmTxt.text = "Plant " + potentialStrain.strainName + "?";

        }
        else
        {
            active = false;
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void ClosePanel()
    {
        // TO-DO: unset everything
        SetConfirmPanelActive(false);
    }

    public void ConfirmAndPlantStrain()
    {
        seedBag.RemoveSeeds(1);
        potentialPlant.SetStrainProfile(potentialStrain);
        potentialPlant.Plant();
        SetConfirmPanelActive(false);
    }

}
