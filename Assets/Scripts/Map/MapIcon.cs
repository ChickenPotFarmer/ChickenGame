using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [Header("Icon Images")]
    public Sprite[] iconsImgs;


    [Header("Setup")]
    public bool lockToScreenBounds;
    public SkinnedMeshRenderer skinRenderer;
    public Camera mainCam;
    public Transform followObj;
    public Image iconImage;

    private Vector3 pos;
    [SerializeField]
    private RectTransform rectTrans;

    private void Start()
    {
        if (!mainCam)
            mainCam = Camera.main;
        //StartCoroutine(FollowRoutine());
    }

    // Refactor note: too many magic numbers.
    private void Update()
    {
        if (followObj != null)
        {
            transform.position = mainCam.WorldToScreenPoint(followObj.position + (Vector3.up * 4));

            if (lockToScreenBounds)
            {
                float x = rectTrans.anchoredPosition.x;
                float y = rectTrans.anchoredPosition.y;

                if (x < -900)
                    x = -900;
                else if (x > 900)
                    x = 900;

                if (y > 500)
                    y = 500;
                else if (y < -500)
                    y = -500;

                //if (skinRenderer)
                //{
                //    if (!skinRenderer.isVisible)
                //        y = -500;
                //}

                pos = new Vector2(x, y);

                rectTrans.anchoredPosition = pos;
            }
        }
        else
            Destroy(gameObject);

    }

    IEnumerator FollowRoutine()
    {
        while (followObj != null)
        {

            transform.position = mainCam.WorldToScreenPoint(followObj.position + (Vector3.up * 4));

            if (lockToScreenBounds)
            {
                float x = rectTrans.anchoredPosition.x;
                float y = rectTrans.anchoredPosition.y;

                if (x < -900)
                    x = -900;
                else if (x > 900)
                    x = 900;

                if (y > 500)
                    y = 500;
                else if (y < -500)
                    y = -500;

                //if (skinRenderer)
                //{
                //    if (!skinRenderer.isVisible)
                //        y = -500;
                //}

                pos = new Vector2(x, y);

                rectTrans.anchoredPosition = pos;
            }

            yield return new WaitForSeconds(0.02f);
        }

        Destroy(gameObject);
    }

    public void SetIcon(int _imageNum)
    {
        iconImage.sprite = iconsImgs[_imageNum];
    }
}
