using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInfo : MonoBehaviour
{
    public Text txt;
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
        transform.position = Input.mousePosition;
    }

    public void SetText(string _txt)
    {
        txt.text = _txt;
    }

    public void SetHoverActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            //cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            //cg.blocksRaycasts = false;
        }
    }
}
