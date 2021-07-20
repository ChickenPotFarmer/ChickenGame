using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jail : MonoBehaviour
{
    [Header("Status")]
    public bool chickenInJail;
    public List<PiggyPatrolController> piggyList;

    [Header("Sentence")]
    public float jailTime;

    [Header("Setup")]
    [SerializeField]
    private Transform jailSpawn;
    [SerializeField]
    private Animator blackoutPanel;

    private ChickenController chicken;
    private TimeLord timeLord;

    public static Jail instance;
    [HideInInspector]
    public GameObject jail;

    private InventoryController inventoryController;

    private void Awake()
    {
        instance = this;
        jail = gameObject;
    }

    private void Start()
    {
        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!timeLord)
            timeLord = TimeLord.instance.timeLord.GetComponent<TimeLord>();
    }

    public void JailChicken()
    {
        if (!chickenInJail)
            StartCoroutine(JailRoutine());
    }

    IEnumerator JailRoutine()
    {
        print("Jail routine started");
        chickenInJail = true;
        chicken.navAgent.enabled = false;
        //play fade to black animation;
        blackoutPanel.SetTrigger("Fade In");
        //timeLord.timeScale = 0.1f;

        // may need a way to remove chicken from game world while on black screen
        // take chicken's inventory;

        yield return new WaitForSecondsRealtime(3.2f);
        chicken.tazerEffects.SetActive(false);

        // Set chicken rotation.
        yield return new WaitForSecondsRealtime(jailTime / 2);
        chicken.transform.position = jailSpawn.position;
        chicken.transform.rotation = jailSpawn.rotation;
        ResetPiggies();

        yield return new WaitForSecondsRealtime(jailTime / 2);
        blackoutPanel.SetTrigger("Fade Out");

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        inventoryController.PoliceWipeInventory();


        //timeLord.timeScale = 1;
        chickenInJail = false;
        chicken.isTazed = false;
        chicken.navAgent.enabled = true;
        chicken.navAgent.isStopped = false;

    }

    private void ResetPiggies()
    {
        for (int i = 0; i < piggyList.Count; i++)
        {
            piggyList[i].ResetPig();
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Jail Chicken"))
            JailChicken();
    }
}
