using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeedDropDown : MonoBehaviour, IPointerExitHandler

{
    private CanvasGroup cg;
    public bool active;
    public Button seedInfoBtn;

    public static SeedDropDown instance;
    [HideInInspector]
    public GameObject seedDropDown;

    private StrainInfoUI strainInfoUI;

    private void Awake()
    {
        instance = this;
        seedDropDown = gameObject;
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        if (!strainInfoUI)
            strainInfoUI = StrainInfoUI.instance.strainInfoUI.GetComponent<StrainInfoUI>();
    }

    public void SetStrainInfoBtn(StrainProfile _strain)
    {
        seedInfoBtn.onClick.AddListener(delegate { SetStrainInfoActive(_strain); });
    }

    public void SetStrainInfoActive(StrainProfile _strain)
    {
        strainInfoUI.SetStrainInfoActive(_strain);
        SetDropdownActive(false);
    }

    public void SetDropdownActive(bool _active)
    {
        if (_active)
        {
            active = true;
            transform.position = Input.mousePosition;
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

    public void OnPointerExit(PointerEventData eventData)
    {
        SetDropdownActive(false);
    }

    
}
