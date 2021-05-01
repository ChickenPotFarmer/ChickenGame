using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryStorage : MonoBehaviour
{
    [Header("Status")]
    public float amountStored;  //change this. Add new small script that tracks type of weed and amount
                                // then make this a List of those small scripts

    [Header("Testing")]
    public KeyCode addToStorageHotkey;
    public float testAmtToAdd;

    [Header("Setup")]
    public float gramsPerBrick;
    public GameObject brickPrefab;
    public Transform spawnPoint;

    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        gramsPerBrick = inventoryController.gramsPerBrick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(addToStorageHotkey))
            AddToStorage(testAmtToAdd);
    }

    public void AddToStorage(float _amt)
    {
        amountStored += _amt;
        float rawBricks = _amt / gramsPerBrick;
        int roundedBricks = Mathf.RoundToInt(rawBricks);
        StartCoroutine(SpawnBricksRoutine(roundedBricks));
    }

    IEnumerator SpawnBricksRoutine(int _bricks)
    {
        GameObject newBrick;
        for (int i = 0; i < _bricks; i++)
        {
            newBrick = Instantiate(brickPrefab, spawnPoint);
            newBrick.transform.position = spawnPoint.position;
            //add force to it
            yield return new WaitForSeconds(0.5f);
        }
    }

}
