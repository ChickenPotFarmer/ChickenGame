using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComparePanel : MonoBehaviour
{
    [Header("Status")]
    public bool active;

    [Header("General Setup")]
    public CompareSlot compareSlot;
    public CanvasGroup cg;

    [Header("Left side")]
    public Text strainNameTxtLeft;
    public Text strainTypeTxtLeft;
    public Text thcTxtLeft;
    public Text terpenePercentTxtLeft;
    public Text primaryTerpeneTxtLeft;
    public Text primaryEffectsTxtLeft;

    [Header("Caryophyllene")]
    public Slider carySliderLeft;
    public Text caryTxtLeft;

    [Header("Limonene")]
    public Slider limonSliderLeft;
    public Text limonTxtLeft;

    [Header("Linalool")]
    public Slider linaSliderLeft;
    public Text linaTxtLeft;

    [Header("Myrcene")]
    public Slider myrcSliderLeft;
    public Text myrcTxtLeft;

    [Header("Pinene")]
    public Slider pineSliderLeft;
    public Text pineTxtLeft;

    [Header("Terpinolene")]
    public Slider terpSliderLeft;
    public Text terpTxtLeft;

    [Header("Right Setup")]
    public Text strainNameTxtRight;
    public Text strainTypeTxtRight;
    public Text thcTxtRight;
    public Text terpenePercentTxtRight;
    public Text primaryTerpeneTxtRight;
    public Text primaryEffectsTxtRight;

    [Header("Caryophyllene")]
    public Slider carySliderRight;
    public Text caryTxtRight;

    [Header("Limonene")]
    public Slider limonSliderRight;
    public Text limonTxtRight;

    [Header("Linalool")]
    public Slider linaSliderRight;
    public Text linaTxtRight;

    [Header("Myrcene")]
    public Slider myrcSliderRight;
    public Text myrcTxtRight;

    [Header("Pinene")]
    public Slider pineSliderRight;
    public Text pineTxtRight;

    [Header("Terpinolene")]
    public Slider terpSliderRight;
    public Text terpTxtRight;

    public void UpdatePanel(StrainProfile _strainLeft, StrainProfile _strainRight)
    {
        // LEFT SIDE
        strainNameTxtLeft.text = _strainLeft.strainName;

        strainTypeTxtLeft.text = _strainLeft.GetStrainType();

        thcTxtLeft.text = "THC: " + (_strainLeft.thcPercent * 100).ToString("n2") + "%";
        terpenePercentTxtLeft.text = "TERPENES: " + (_strainLeft.totalTerpenesPercent * 100).ToString("n2") + "%";

        primaryTerpeneTxtLeft.text = _strainLeft.GetPrimaryTerpene();

        primaryEffectsTxtLeft.text = _strainLeft.primaryEffect + ", " + _strainLeft.secondaryEffect;

        carySliderLeft.value = _strainLeft.caryophyllene;
        caryTxtLeft.text = (_strainLeft.caryophyllene * 100).ToString("n2") + "%";

        limonSliderLeft.value = _strainLeft.limonene;
        limonTxtLeft.text = (_strainLeft.limonene * 100).ToString("n2") + "%";

        linaSliderLeft.value = _strainLeft.linalool;
        linaTxtLeft.text = (_strainLeft.linalool * 100).ToString("n2") + "%";

        myrcSliderLeft.value = _strainLeft.myrcene;
        myrcTxtLeft.text = (_strainLeft.myrcene * 100).ToString("n2") + "%";

        pineSliderLeft.value = _strainLeft.pinene;
        pineTxtLeft.text = (_strainLeft.pinene * 100).ToString("n2") + "%";

        terpSliderLeft.value = _strainLeft.terpinolene;
        terpTxtLeft.text = (_strainLeft.terpinolene * 100).ToString("n2") + "%";


        // RIGHT SIDE
        strainNameTxtRight.text = _strainRight.strainName;

        strainTypeTxtRight.text = _strainRight.GetStrainType();

        thcTxtRight.text = "THC: " + (_strainRight.thcPercent * 100).ToString("n2") + "%";
        terpenePercentTxtRight.text = "TERPENES: " + (_strainRight.totalTerpenesPercent * 100).ToString("n2") + "%";

        primaryTerpeneTxtRight.text = _strainRight.GetPrimaryTerpene();

        primaryEffectsTxtRight.text = _strainRight.primaryEffect + ", " + _strainRight.secondaryEffect;

        carySliderRight.value = _strainRight.caryophyllene;
        caryTxtRight.text = (_strainRight.caryophyllene * 100).ToString("n2") + "%";

        limonSliderRight.value = _strainRight.limonene;
        limonTxtRight.text = (_strainRight.limonene * 100).ToString("n2") + "%";

        linaSliderRight.value = _strainRight.linalool;
        linaTxtRight.text = (_strainRight.linalool * 100).ToString("n2") + "%";

        myrcSliderRight.value = _strainRight.myrcene;
        myrcTxtRight.text = (_strainRight.myrcene * 100).ToString("n2") + "%";

        pineSliderRight.value = _strainRight.pinene;
        pineTxtRight.text = (_strainRight.pinene * 100).ToString("n2") + "%";

        terpSliderRight.value = _strainRight.terpinolene;
        terpTxtRight.text = (_strainRight.terpinolene * 100).ToString("n2") + "%";
    }

    public void ClosePanel()
    {
        
        SetComparePanelActive(false);
    }

    public void SetComparePanelActive(StrainProfile _strainLeft, StrainProfile _strainRight)
    {
        UpdatePanel(_strainLeft, _strainRight);
        active = true;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void SetComparePanelActive(bool _active)
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
