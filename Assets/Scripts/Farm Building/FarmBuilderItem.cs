using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmBuilderItem : MonoBehaviour
{
    [Header("Settings")]
    public float storeCost;

    [Header("Setup")]
    public int objectPrefabInt;

    private Button itemBtn;
    private FarmStore farmStore;

    private void Start()
    {
        if (!farmStore)
            farmStore = FarmStore.instance.farmStore.GetComponent<FarmStore>();

        SetButton();
    }

    private void SetButton()
    {
        itemBtn = GetComponent<Button>();
        itemBtn.onClick.AddListener(delegate { farmStore.SetSelectedItem(objectPrefabInt); });
    }
}
