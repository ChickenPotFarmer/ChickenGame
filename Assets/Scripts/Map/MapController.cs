using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapController : MonoBehaviour
{
    [Header("Status")]
    public bool mapActive;

    [Header("Settings")]
    public float camSpeed;

    [Header("Setup")]
    public CanvasGroup iconsCg;
    public Vector3 mapStartPos;
    public GameObject mapCam;
    public Rigidbody camRb;
    public CinemachineVirtualCamera camComp;

    private Vector3 forceDir;
    private InputController inputController;

    public static MapController instance;
    [HideInInspector]
    public GameObject mapController;

    private void Awake()
    {
        instance = this;
        mapController = gameObject;
    }

    private void Start()
    {
        if (!inputController)
            inputController = InputController.instance.inputController.GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        if (mapActive)
        {
            if (Input.GetKey("w"))
            {
                forceDir = new Vector3(0, 0, 1) * camSpeed;
            }
            else if (Input.GetKey("s"))
            {
                forceDir = new Vector3(0, 0, -1) * camSpeed;
            }
            else if (Input.GetKey("a"))
            {
                forceDir = new Vector3(-1, 0, 0) * camSpeed;
            }
            else if (Input.GetKey("d"))
            {
                forceDir = new Vector3(1, 0, 0) * camSpeed;
            }
            else
                forceDir = new Vector3(0, 0, 0) * camSpeed;
            camRb.AddForce(forceDir, ForceMode.Impulse);

            if (Input.mouseScrollDelta.y > 0)
            {
                camComp.m_Lens.FieldOfView -= 2;
                if (camComp.m_Lens.FieldOfView < 8)
                    camComp.m_Lens.FieldOfView = 8;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                camComp.m_Lens.FieldOfView += 2;
                if (camComp.m_Lens.FieldOfView > 54)
                    camComp.m_Lens.FieldOfView = 54;
            }

        }
    }

    public void ToggleMap()
    {
        if (mapActive)
        {
            mapCam.SetActive(false);
            mapActive = false;
            inputController.mapActive = false;
            SetIconsActive(false);
            //mapCam.transform.position = mapStartPos;
        }
        else
        {
            mapCam.SetActive(true);
            mapActive = true;
            inputController.mapActive = true;
            SetIconsActive(true);

        }
    }

    private void SetIconsActive(bool _active)
    {
        if (_active)
        {
            iconsCg.alpha = 1;
            iconsCg.interactable = true;
            iconsCg.blocksRaycasts = true;
        }
        else
        {
            iconsCg.alpha = 0;
            iconsCg.interactable = false;
            iconsCg.blocksRaycasts = false;
        }
    }
        
}
