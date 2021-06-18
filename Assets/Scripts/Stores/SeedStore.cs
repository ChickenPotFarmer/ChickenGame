using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedStore : MonoBehaviour
{
    // These are buttons, not SeedBags
    [Header("Common Seeds")]
    public GameObject[] commonSeeds;

    [Header("Rare Seeds")]
    public GameObject[] rareSeeds;

    [Header("Legendary Seeds")]
    public GameObject[] legendarySeeds;

    [Header("Setup")]
    public CanvasGroup confirmPanelCg;
    public Button btnComp;

    public static SeedStore instance;
    [HideInInspector]
    public GameObject seedStore;

    private void Awake()
    {
        instance = this;
        seedStore = gameObject;
    }

    private void SetConfirmActive(bool _active)
    {
        if (_active)
        {
            confirmPanelCg.alpha = 1;
            confirmPanelCg.interactable = true;
            confirmPanelCg.blocksRaycasts = true;
        }
        else
        {
            confirmPanelCg.alpha = 0;
            confirmPanelCg.interactable = false;
            confirmPanelCg.blocksRaycasts = false;
        }
    }

}
