using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RadialIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Icon Tag")]
    public string iconTag;

    private RadialMenu radialMenu;

    private void Start()
    {
        if (!radialMenu)
            radialMenu = RadialMenu.instance.radialMenu.GetComponent<RadialMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        radialMenu.currentItemSelected = iconTag;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        radialMenu.currentItemSelected = null;

    }
}
