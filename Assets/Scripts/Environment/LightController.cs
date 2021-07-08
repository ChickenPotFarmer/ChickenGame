using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light mainLight;
    public Transform lightTarget;

    [Header("Night Settings")]
    public float nightIntensity;
    public Color nightColor;

    [Header("Day Settings")]
    public float dayIntensity;
    public Color dayColor;

    private void Update()
    {
        //Vector3 dir = new Vector3(lightTarget.);
        transform.LookAt(lightTarget.position);
    }
}
