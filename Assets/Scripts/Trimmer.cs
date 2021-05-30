using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trimmer : MonoBehaviour
{
    [Header("Status")]
    public bool trimmerOn;
    public bool autoTrimOn;

    [Header("Setup")]
    public GameObject trimmerModel;
    public GameObject trimmingsPrefab;

    [Header("Current Trimmer Settings")]
    public int minTrimmings;
    public int maxTrimmings;
    public float trimmerReach;

    private WeedPlant selectedPlant;

    public static Trimmer instance;
    [HideInInspector]
    public GameObject trimmer;

    private void Start()
    {
        instance = this;
        trimmer = gameObject;

        if (trimmerOn)
            trimmerModel.SetActive(true);
        else
            trimmerModel.SetActive(false);
    }

    //private void Update()
    //{
    //    if (trimmerOn)
    //    {
    //        if (Input.GetButtonDown("Interact"))
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
    //            {
    //                if (hit.collider != null)
    //                {
    //                    if (hit.collider.gameObject.CompareTag("Weed Plant"))
    //                    {
    //                        selectedPlant = hit.collider.GetComponent<WeedPlant>();
    //                        if (selectedPlant.fullyGrown)
    //                        {
    //                            TrimPlant(selectedPlant);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void TrimPlant(WeedPlant _plant)
    {
        selectedPlant = _plant;

        int trimmings = Random.Range(minTrimmings, maxTrimmings + 1);
        selectedPlant.trimmed = true;

        StartCoroutine((SpawnRoutine(trimmings)));
    }

    IEnumerator SpawnRoutine(int _amt)
    {
        GameObject trim;
        for (int i = 0; i < _amt; i++)
        {
            yield return new WaitForSeconds(0.2f);
            trim = Instantiate(trimmingsPrefab);
            trim.transform.position = selectedPlant.transform.position;
        }
    }

    public bool TrimmerIsOn()
    {
        bool on = false;

        if (trimmerOn || autoTrimOn)
            on = true;

        return on;
    }

    public void ToggleTrimmer()
    {
        if (trimmerOn)
        {
            trimmerOn = false;
            trimmerModel.SetActive(false);
        }
        else
        {
            trimmerOn = true;
            trimmerModel.SetActive(true);
        }
    }
}
