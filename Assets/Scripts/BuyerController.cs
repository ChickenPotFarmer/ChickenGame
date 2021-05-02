using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour
{
    [Header("Buyers")]
    public GameObject[] buyerPrefabs;
    public Buyer hoveringOver;

    [Header("Spawn Locations")]
    public Transform spawnLocationsParent;
    public Transform[] spawnLocations;

    [Header("Status")]
    public bool hoverActive;

    private ToDoController toDoController;

    public static BuyerController instance;
    [HideInInspector]
    public GameObject buyerContoller;

    private void Awake()
    {
        instance = this;
        buyerContoller = gameObject;
    }

    private void Start()
    {
        if (!toDoController)
            toDoController = ToDoController.instance.toDoController.GetComponent<ToDoController>();

        spawnLocations = new Transform[spawnLocationsParent.childCount];
        for (int i = 0; i < spawnLocationsParent.childCount; i++)
        {
            spawnLocations[i] = spawnLocationsParent.GetChild(i);
        }
    }

    //private void Update()
    //{
    //    if (hoverActive)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.collider != null)
    //            {
    //                if (hit.collider.gameObject.CompareTag("Buyer"))
    //                {
    //                    hoveringOver = hit.collider.gameObject.GetComponent<Buyer>();
    //                    hoveringOver.SetHoverInfoActive(true);

    //                    if (Input.GetMouseButtonDown(0))
    //                    {
    //                        hoveringOver.DeliverWeed();
    //                    }
    //                }
    //                else
    //                {
    //                    if (hoveringOver != null)
    //                    {
    //                        hoveringOver.SetHoverInfoActive(false);
    //                        hoveringOver = null;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void SpawnBuyer(string _buyerName, float _amtRequested, float _totalPay, ToDoObject _toDo)
    {
        int randSpawn = Random.Range(0, spawnLocations.Length);
        int randBuyer = Random.Range(0, buyerPrefabs.Length);
        GameObject newBuyer = Instantiate(buyerPrefabs[randBuyer], spawnLocations[randSpawn]);
        newBuyer.transform.position = spawnLocations[randSpawn].position;
        newBuyer.GetComponent<Buyer>().SetInfo(_buyerName, _amtRequested, _totalPay, _toDo);
    }
}
