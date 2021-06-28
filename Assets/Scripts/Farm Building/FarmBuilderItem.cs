using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmBuilderItem : MonoBehaviour
{
    [Header("Settings")]
    public string itemName;
    public float storeCost;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI costTxt;

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
        itemBtn.onClick.AddListener(delegate { farmStore.SetSelectedItem(objectPrefabInt, storeCost); });
        costTxt.text = "$" + storeCost.ToString("n0");
        titleTxt.text = itemName;
    }
}
