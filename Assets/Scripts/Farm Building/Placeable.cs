using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Things to activate")]
    public Transform activateObjectsParent;
    public List<GameObject> activateObjects;
    public List<GameObject> objectLayersToSetDefault;
    public StorageCrate storageCrate;
    public DryerController dryer;
    public bool setAllLayersDefault;

    [Header("Particle Effect")]
    public GameObject particleEffect;

    [Header("Status")]
    public bool placed;
    public bool placementOk = true;
    public List<Collider> triggers;

    [Header("Materials")]
    public MeshRenderer meshRenderer;
    public Material okMat;
    public Material nopeMat;

    private FarmStore farmStore;

    private void Start()
    {
        if (!farmStore)
            farmStore = FarmStore.instance.farmStore.GetComponent<FarmStore>();


        if (placed)
        {
            meshRenderer.enabled = false;
            placed = true;

            if (activateObjectsParent != null)
            {
                for (int i = 0; i < activateObjectsParent.childCount; i++)
                {
                    if (!activateObjects.Contains(activateObjectsParent.GetChild(i).gameObject))
                        activateObjects.Add(activateObjectsParent.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < activateObjects.Count; i++)
            {
                activateObjects[i].SetActive(true);
            }

            if (storageCrate)
                storageCrate.PlaceCrate();

            //if (particleEffect)
            //{
            //    GameObject particles = Instantiate(particleEffect, transform.parent);
            //    Vector3 pos = new Vector3(0, 0, 0);
            //    particles.transform.position = transform.parent.position;
            //}

            if (setAllLayersDefault)
            {
                transform.parent.gameObject.layer = 0;

                for (int i = 0; i < objectLayersToSetDefault.Count; i++)
                {
                    objectLayersToSetDefault[i].layer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != transform.parent)
        {
            if (!triggers.Contains(other))
            {
                triggers.Add(other);

                meshRenderer.material = nopeMat;
                placementOk = false;
            }
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

    public bool BuyAndPlaceObject()
    {
        if (farmStore.BuySelected())
        {
            PlaceObject();
            return true;
        }
        else
            return false;
    }

    public void PlaceObject()
    {
        meshRenderer.enabled = false;
        placed = true;

        if (activateObjectsParent != null)
        {
            
            for (int i = 0; i < activateObjectsParent.childCount; i++)
            {
                if (!activateObjects.Contains(activateObjectsParent.GetChild(i).gameObject))
                    activateObjects.Add(activateObjectsParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < activateObjects.Count; i++)
        {
            activateObjects[i].SetActive(true);
        }

        if (storageCrate)
            storageCrate.PlaceCrate();
        else if (dryer)
            dryer.PlaceDryer();

        if (particleEffect)
        {
            GameObject particles = Instantiate(particleEffect, transform.parent);
            Vector3 pos = new Vector3(0, 0, 0);
            particles.transform.position = transform.parent.position;
        }

        if (setAllLayersDefault)
        {
            transform.parent.gameObject.layer = 0;

            for (int i = 0; i < objectLayersToSetDefault.Count; i++)
            {
                objectLayersToSetDefault[i].layer = 0;
            }
        }
    }
}
