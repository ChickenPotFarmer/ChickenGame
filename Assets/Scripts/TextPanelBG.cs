using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPanelBG : MonoBehaviour
{
    public Text txt;
    private RectTransform rectTransfrom;

    private void Start()
    {
        if (!rectTransfrom)
            rectTransfrom = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransfrom.anchoredPosition = txt.transform.position;
        rectTransfrom.sizeDelta = new Vector2(100, 100);
    }

}
