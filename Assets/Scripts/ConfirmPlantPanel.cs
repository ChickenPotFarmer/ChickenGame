using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPlantPanel : MonoBehaviour
{
    [Header("Setup")]
    public CanvasGroup cg;
    public bool active;
    public StrainProfile potentialStrain;
    public WeedPlant potentialPlant;

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
        }
        else
        {
            active = false;
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void ConfirmAndPlantStrain()
    {

    }

}
