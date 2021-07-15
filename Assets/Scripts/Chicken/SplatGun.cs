using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatGun : MonoBehaviour
{
    [Header("Status")]
    public bool cannonOn;

    [Header("Setup")]
    public Transform firePoint;
    public GameObject projectile;
    public GameObject cannonModel;

    public static SplatGun instance;
    [HideInInspector]
    public GameObject splatGun;

    [SerializeField]
    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        instance = this;
        splatGun = gameObject;
    }

    //private void Start()
    //{
    //    if (cannonOn)
    //        cannonModel.SetActive(true);
    //    else
    //        cannonModel.SetActive(false);
    //}

    public void ToggleCannon(bool _active)
    {
        cannonModel.SetActive(_active);

        cannonOn = _active;

        if (_active)
            thirdPersonController.SplatCannonLock();
        else
            thirdPersonController.SplatCannonUnlock();
    }


    public void Fire(Vector3 _target)
    {
        //if (seedSlot.HasItem())
        //{
        GameObject shot = Instantiate(projectile);
        shot.transform.position = firePoint.position;

        SplatAmmo newSplat = shot.GetComponent<SplatAmmo>();

        newSplat.target = thirdPersonController.targetImage.position;



        //if (seedBagItem.AddAmount(-1))
        //{
        //    ResetCannon();
        //}
        //}
    }
}
