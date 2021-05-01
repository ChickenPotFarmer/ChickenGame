using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickInventory : MonoBehaviour
{
    [Header("Brick Slots")]
    public Transform[] brickSlots;
    public Transform brickParent;
    public bool slotsFull;

    [Header("Brick Prefab")]
    public GameObject brickPrefab;

    private int nextOpenSlot;

    private void Start()
    {
        brickSlots = new Transform[brickParent.childCount];
        for (int i = 0; i < brickParent.childCount; i++)
        {
            brickSlots[i] = brickParent.GetChild(i);
        }
    }

    public void AddBrick()
    {
        GameObject newBrick = Instantiate(brickPrefab, brickSlots[nextOpenSlot]);
        newBrick.transform.position = brickSlots[nextOpenSlot].position;
        nextOpenSlot++;
        if (nextOpenSlot == brickSlots.Length)
            slotsFull = true;
    }

    public void RemoveAllBricks()
    {
        try
        {
            for (int i = 0; i < brickSlots.Length; i++)
            {
                Destroy(brickSlots[i].GetChild(0).gameObject);
            }
        }
        catch
        {
            Debug.LogWarning("No Bricks To Remove");
        }

        slotsFull = false;
        nextOpenSlot = 0;
    }
}
