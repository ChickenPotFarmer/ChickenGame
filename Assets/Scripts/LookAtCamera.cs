using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool invert;
    private void Update()
    {
        Vector3 dir = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.LookAt(dir);
        if (invert)
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
    }
}
