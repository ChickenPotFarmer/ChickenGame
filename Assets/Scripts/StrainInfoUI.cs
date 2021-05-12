using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrainInfoUI : MonoBehaviour
{
    [Header("Status")]
    public bool active;

    [Header("Setup")]
    public Text strainNameTxt;
    public CanvasGroup cg;

    [Header("Caryophyllene")]
    public Slider carySlider;
    public Text caryTxt;

    [Header("Limonene")]
    public Slider limonSlider;
    public Text limonTxt;

    [Header("Linalool")]
    public Slider linaSlider;
    public Text linaTxt;

    [Header("Myrcene")]
    public Slider myrcSlider;
    public Text myrcTxt;

    [Header("Pinene")]
    public Slider pineSlider;
    public Text pineTxt;

    [Header("Terpinolene")]
    public Slider terpSlider;
    public Text terpTxt;

    public static StrainInfoUI instance;
    [HideInInspector]
    public GameObject strainInfoUI;

    private void Awake()
    {
        instance = this;
        strainInfoUI = gameObject;
    }

    public void UpdatePanel(StrainProfile _strain)
    {
        strainNameTxt.text = _strain.strainName;

        carySlider.value = _strain.caryophyllene;
        caryTxt.text = (_strain.caryophyllene * 100).ToString("n2") + "%";

        limonSlider.value = _strain.limonene;
        limonTxt.text = (_strain.limonene * 100).ToString("n2") + "%";

        linaSlider.value = _strain.linalool;
        linaTxt.text = (_strain.linalool * 100).ToString("n2") + "%";

        myrcSlider.value = _strain.myrcene;
        myrcTxt.text = (_strain.myrcene * 100).ToString("n2") + "%";

        pineSlider.value = _strain.pinene;
        pineTxt.text = (_strain.pinene * 100).ToString("n2") + "%";

        terpSlider.value = _strain.terpinolene;
        terpTxt.text = (_strain.terpinolene * 100).ToString("n2") + "%";
    }

    public void ClosePanel()
    {
        SetStrainInfoActive(false);
    }

    public void SetStrainInfoActive(StrainProfile _strain)
    {
        UpdatePanel(_strain);
        active = true;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void SetStrainInfoActive(bool _active)
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
}
