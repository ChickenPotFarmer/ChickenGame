using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [Header("Icon Images")]
    public Sprite[] iconsImgs;


    [Header("Setup")]
    public Camera mainCam;
    public Transform followObj;
    public Image iconImage;

    private void Start()
    {
        if (!mainCam)
            mainCam = Camera.main;
        StartCoroutine(FollowRoutine());
    }

    IEnumerator FollowRoutine()
    {
        do
        {
            transform.position = mainCam.WorldToScreenPoint(followObj.position);

            yield return new WaitForSeconds(0.02f);
        } while (true);
    }

    public void SetIcon(int _imageNum)
    {
        iconImage.sprite = iconsImgs[_imageNum];
    }
}
