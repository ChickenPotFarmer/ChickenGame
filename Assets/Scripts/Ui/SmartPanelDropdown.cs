using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmartPanelDropdown : MonoBehaviour, IPointerExitHandler
{
    private CanvasGroup cg;
    public bool active;
    public Transform container;


    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }


    public void SetDropdownActive(bool _active)
    {
        if (container.childCount != 0)
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetDropdownActive(false);
        print("dropdown exit");
    }
}
