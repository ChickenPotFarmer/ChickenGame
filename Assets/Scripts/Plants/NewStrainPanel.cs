using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewStrainPanel : MonoBehaviour
{
    [Header("Status")]
    public List<StrainProfile> newStrainStack = new List<StrainProfile>();

    [Header("Setup")]
    [SerializeField] private StrainProfile testStrain;
    [SerializeField] private StrainInfoUI strainInfoUI;
    [SerializeField] private Animator strainNameAnimator;
    [SerializeField] private TMP_InputField strainNameInput;
    private UniqueIdMaster uniqueIdMaster;


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
            strainInfoUI.SetStrainInfoActive(newStrainStack[0]);
            SetStrainNamePanelActive(true);

        }
        else
        {
            newStrainStack.Add(_strain);
            //newStrain.SetStrain(newStrainStack[0]);


        }
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

        strainInfoUI.UpdatePanel(newStrainStack[0]);

        newStrainStack.Remove(newStrainStack[0]);

        // put this in a routine
        //if (newStrainStack.Count != 0)
        //{
        //    newStrain = newStrainStack[0];

        //    SetStrainNamePanelActive(true);
        //}

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "WTF"))
            strainInfoUI.UpdatePanel(newStrainStack[0]);


        //if (GUI.Button(new Rect(110, 10, 100, 50), "Rename strain 2"))
        //    SetNewName("42042069", "IT WORKED TWICE!");
    }
}
