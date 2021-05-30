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

    public void SpawnBuyer(Email _email, ToDoObject _toDo)
    {
        string _buyerName = _email.fromName;
        float _amtRequested = _email.orderAmt;
        float _totalPay = _email.totalPay;

        int randSpawn = Random.Range(0, spawnLocations.Length);
        int randBuyer = Random.Range(0, buyerPrefabs.Length);
        GameObject newBuyer = Instantiate(buyerPrefabs[randBuyer], spawnLocations[randSpawn]);
        newBuyer.transform.position = spawnLocations[randSpawn].position;
        newBuyer.GetComponent<Buyer>().SetInfo(_email, _toDo);
    }
}
