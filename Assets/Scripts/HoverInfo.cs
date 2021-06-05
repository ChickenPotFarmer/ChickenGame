using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInfo : MonoBehaviour
{
    [Header("UI")]
    public Text nameTxt;
    public Text typeTxt;
    public Text thcTxt;
    public Text terpeneTxt;

    [Header("Status")]
    public bool isActive;

    private CanvasGroup cg;

    public static HoverInfo instance;
    [HideInInspector]
    public GameObject hoverInfo;

    private void Awake()
    {
        instance = this;
        hoverInfo = gameObject;
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (isActive)
            transform.position = Input.mousePosition;
    }

    public void SetText(string[] _txt)
    {
        nameTxt.text = _txt[0];

        if (_txt[1] != "NULL")
        {
            if (!typeTxt.gameObject.activeInHierarchy)
                typeTxt.gameObject.SetActive(true);

            typeTxt.text = _txt[1];
        }
        else
            typeTxt.gameObject.SetActive(false);

        if (_txt[2] != "NULL")
        {
            if (!thcTxt.gameObject.activeInHierarchy)
                thcTxt.gameObject.SetActive(true);

            thcTxt.text = _txt[2];
        }
        else
            thcTxt.gameObject.SetActive(false);


        if (_txt[3] != "NULL")
        {
            if (!terpeneTxt.gameObject.activeInHierarchy)
                terpeneTxt.gameObject.SetActive(true);

            terpeneTxt.text = _txt[3];
        }
        else
            terpeneTxt.gameObject.SetActive(false);

    }

    public void SetHoverActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            isActive = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            isActive = false;
        }
    }
}
