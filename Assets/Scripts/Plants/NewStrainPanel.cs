using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;

public class NewStrainPanel : MonoBehaviour
{
    [Header("Status")]
    public List<StrainProfile> newStrainStack = new List<StrainProfile>();

    [Header("Panel Comps")]
    [SerializeField] private TextMeshProUGUI motherName;
    [SerializeField] private TextMeshProUGUI motherType;
    [SerializeField] private TextMeshProUGUI motherThc;
    [SerializeField] private TextMeshProUGUI motherTerpenes;

    [SerializeField] private TextMeshProUGUI fatherName;
    [SerializeField] private TextMeshProUGUI fatherType;
    [SerializeField] private TextMeshProUGUI fatherThc;
    [SerializeField] private TextMeshProUGUI fatherTerpenes;

    [SerializeField] private TextMeshProUGUI newStrainName;
    [SerializeField] private TextMeshProUGUI newStrainType;
    [SerializeField] private TextMeshProUGUI newStrainThc;
    [SerializeField] private TextMeshProUGUI newStrainTerpenes;

    [Header("Setup")]
    [SerializeField] private PieChart pieChart;
    [SerializeField] private PieChart motherPieChart;
    [SerializeField] private PieChart fatherPieChart;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Animator strainNameAnimator;
    [SerializeField] private TMP_InputField strainNameInput;
    private UniqueIdMaster uniqueIdMaster;
    private bool newStrainRoutineActive;


    private void Start()
    {
        if (!uniqueIdMaster)
            uniqueIdMaster = UniqueIdMaster.instance.uniqueIdMaster.GetComponent<UniqueIdMaster>();
    }

    public void OpenNewStrainPanel(StrainProfile _strain)
    {
        if (newStrainStack.Count == 0)
        {
            newStrainStack.Add(_strain);
            StartCoroutine(SetNewStrainPanelActive(newStrainStack[0]));
        }
        else
        {
            newStrainStack.Add(_strain);
        }
    }

    public void ShowStrain(StrainProfile _strain)
    {
        motherName.text = _strain.mother.strainName;
        motherType.text = _strain.mother.GetStrainType();
        motherThc.text = _strain.mother.GetReaderFriendlyThcContent();
        motherTerpenes.text = _strain.mother.GetReaderFriendlyTerpeneContent();

        fatherName.text = _strain.father.strainName;
        fatherType.text = _strain.father.GetStrainType();
        fatherThc.text = _strain.father.GetReaderFriendlyThcContent();
        fatherTerpenes.text = _strain.father.GetReaderFriendlyTerpeneContent();

        newStrainName.text = _strain.strainName;
        newStrainType.text = _strain.GetStrainType();
        newStrainThc.text = _strain.GetReaderFriendlyThcContent();
        newStrainTerpenes.text = _strain.GetReaderFriendlyTerpeneContent();

        pieChart.ChangeValue(0, _strain.caryophyllene);
        pieChart.ChangeValue(1, _strain.limonene);
        pieChart.ChangeValue(2, _strain.linalool);
        pieChart.ChangeValue(3, _strain.myrcene);
        pieChart.ChangeValue(4, _strain.pinene);
        pieChart.ChangeValue(5, _strain.terpinolene);

        motherPieChart.ChangeValue(0, _strain.mother.caryophyllene);
        motherPieChart.ChangeValue(1, _strain.mother.limonene);
        motherPieChart.ChangeValue(2, _strain.mother.linalool);
        motherPieChart.ChangeValue(3, _strain.mother.myrcene);
        motherPieChart.ChangeValue(4, _strain.mother.pinene);
        motherPieChart.ChangeValue(5, _strain.mother.terpinolene);

        fatherPieChart.ChangeValue(0, _strain.father.caryophyllene);
        fatherPieChart.ChangeValue(1, _strain.father.limonene);
        fatherPieChart.ChangeValue(2, _strain.father.linalool);
        fatherPieChart.ChangeValue(3, _strain.father.myrcene);
        fatherPieChart.ChangeValue(4, _strain.father.pinene);
        fatherPieChart.ChangeValue(5, _strain.father.terpinolene);

        pieChart.SetAllDirty();
        pieChart.UpdateIndicators();
        motherPieChart.SetAllDirty();
        motherPieChart.UpdateIndicators();
        fatherPieChart.SetAllDirty();
        fatherPieChart.UpdateIndicators();
    }



    public IEnumerator SetNewStrainPanelActive(StrainProfile _strain)
    {
        newStrainRoutineActive = true;
        // update info
        ShowStrain(_strain);
        SetPanelActive(true);
        SetStrainNamePanelActive(true);

        if (newStrainStack.Count > 1)
        {
            do
            {
                yield return new WaitForSeconds(1);
            } while (newStrainRoutineActive);
            

            StartCoroutine(SetNewStrainPanelActive(newStrainStack[0]));
        }


    }

    public void CloseNewStrainPanel()
    {
        SetPanelActive(false);
        newStrainStack.RemoveAt(0);
        newStrainRoutineActive = false;
    }

    private void SetStrainNamePanelActive(bool _active)
    {
        if(_active)
        {
            print("set active");
            strainNameAnimator.SetTrigger("Open");
        }   
        else
        {
            strainNameAnimator.SetTrigger("Close");

            print("set inactive");

        }
    }

    public void SetNewStrainName()
    {
        uniqueIdMaster.SetNewName(newStrainStack[0].strainID, strainNameInput.text);
        newStrainStack[0].strainName = strainNameInput.text;
        try
        {
            newStrainStack[0].itemComp.SetItemName();
        }
        catch
        {
            Debug.LogWarning("newStream.itemComp bug.");
        }
        SetStrainNamePanelActive(false);
        ShowStrain(newStrainStack[0]);


        // put this in a routine
        //if (newStrainStack.Count != 0)
        //{
        //    newStrain = newStrainStack[0];

        //    SetStrainNamePanelActive(true);
        //}

    }

    public void SetPanelActive(bool _active)
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
        }
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 100, 50), "WTF"))
    //    {
    //        pieChart.UpdateIndicators();
    //        motherPieChart.UpdateIndicators();
    //        fatherPieChart.UpdateIndicators();
    //    }
    //}
}
