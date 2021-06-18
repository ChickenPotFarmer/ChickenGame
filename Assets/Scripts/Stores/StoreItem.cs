using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItem : MonoBehaviour
{
    public bool isStrain;
    public float storeCost;
    public GameObject objectPrefab;
    public Button itemBtn;
    public TextMeshProUGUI nameTxt;
    private SeedStore seedStore;

    private void Start()
    {
        if (!seedStore)
            seedStore = SeedStore.instance.seedStore.GetComponent<SeedStore>();

        SetButton();
    }

    private void SetButton()
    {
        itemBtn.onClick.AddListener(delegate { seedStore.SetSelectedItem(this); });
        if (isStrain)
            nameTxt.text = objectPrefab.GetComponent<StrainProfile>().strainName;
    }
}
