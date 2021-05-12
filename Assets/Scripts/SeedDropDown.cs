using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedDropDown : MonoBehaviour, IPointerExitHandler

{
    private CanvasGroup cg;
    public bool active;

    public static SeedDropDown instance;
    [HideInInspector]
    public GameObject seedDropDown;

    private void Awake()
    {
        instance = this;
        seedDropDown = gameObject;
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
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
