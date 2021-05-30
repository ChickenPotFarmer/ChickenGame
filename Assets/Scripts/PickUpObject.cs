using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public float MinDistance = 10;
    private SphereCollider col;
    public float Speed = 7;
    public ChickenController chicken;
    private bool captured;
    private bool pickedUp;
    private Animator animator;
    public bool pickUpEnabled;
    private bool soundPlayed;
    private bool pickUpSoundPlayed;
    //private SoundManager soundManager;
    public string pickUpType;
    public float amtToAddToPlayer;

    private void Start()
    {
        col = gameObject.GetComponent<SphereCollider>();
        chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();
        //soundManager = SoundManager.instance.soundManager.GetComponent<SoundManager>();

        if (!animator)
            animator = gameObject.GetComponent<Animator>();

        pickUpEnabled = false;
        Invoke("EnablePickUp", 1.5f);

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce(new Vector3(Random.Range(-1,1), Random.Range(1, 5), Random.Range(-1,1)), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)), ForceMode.Impulse);
    }

    private void EnablePickUp() 
    {
        pickUpEnabled = true; 
    }

    void Update()
    {
        if (pickUpEnabled)
        {
            float distance = Vector3.Distance(transform.position, chicken.transform.position);
            if (distance <= MinDistance)
            {
                //if(!soundPlayed)
                //{
                //    soundPlayed = true;
                //    gameObject.GetComponent<AudioSource>().pitch = Random.Range(0, 3f);
                //    gameObject.GetComponent<AudioSource>().Play();
                //}

                Vector3 destination = chicken.pickupSlot.position;
                this.transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
                captured = true;

                if (captured)
                {
                    if (col.enabled)
                        col.enabled = false;

                    if (Vector3.Distance(transform.position, destination) <= 0.5f)
                    {
                        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                        animator.Play("Shrink");

                        //if (!pickUpSoundPlayed)
                        //{
                        //    pickUpSoundPlayed = true;
                        //    soundManager.pickUpBeep.Play();
                        //}

                        if (!pickedUp)
                        {
                            pickedUp = true;
                            StartCoroutine(AddToInventory());
                        }
                    }
                }
            }
        }
        
    }

    IEnumerator AddToInventory()
    {
        //switch(pickUpType)
        //{
        //    case "POWER":
        //        player.playerStatus.power += amtToAddToPlayer;
        //        break;

        //    case "HEALTH":
        //        player.playerStatus.health += amtToAddToPlayer;
        //        break;

        //    case "CHIP":
        //        player.playerInventory.AddChips(amtToAddToPlayer);
        //        break;
        //}
        yield return new WaitForSeconds(1);
        //add to scriptable object inventory
        Destroy(gameObject);
    }
}
