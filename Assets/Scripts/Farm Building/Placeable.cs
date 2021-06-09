﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Things to activate")]
    public Transform activateObjectsParent;
    public List<GameObject> activateObjects;

    [Header("Particle Effect")]
    public GameObject particleEffect;

    [Header("Status")]
    public bool placementOk = true;
    public List<Collider> triggers;

    [Header("Materials")]
    public MeshRenderer meshRenderer;
    public Material okMat;
    public Material nopeMat;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggers.Contains(other))
        {
            triggers.Add(other);

            meshRenderer.material = nopeMat;
            placementOk = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (triggers.Contains(other))
        {
            triggers.Remove(other);
        }

        if (triggers.Count == 0)
        {
            meshRenderer.material = okMat;
            placementOk = true;
        }

    }

    public void PlaceObject()
    {
        meshRenderer.enabled = false;

        if (activateObjectsParent != null)
        {
            for (int i = 0; i < activateObjectsParent.childCount; i++)
            {
                activateObjects.Add(activateObjectsParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < activateObjects.Count; i++)
        {
            activateObjects[i].SetActive(true);
        }

        if (particleEffect)
        {
            GameObject particles = Instantiate(particleEffect, transform.parent);
            Vector3 pos = new Vector3(0, 0, 0);
            particles.transform.position = transform.parent.position;
        }

    }
}
