using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmBuilderItem : MonoBehaviour
{
    [Header("Settings")]
    public bool isStrain;
    public float storeCost;

    [Header("Setup")]
    public GameObject objectPrefab;
    public Button itemBtn;

    private FarmStore farmStore;

    private void Start()
    {
        if (!farmStore)
            farmStore = FarmStore.instance.farmStore.GetComponent<FarmStore>();

        SetButton();
    }

    private void SetButton()
    {
        itemBtn.onClick.AddListener(delegate { farmStore.SetSelectedItem(this); });
    }
}
