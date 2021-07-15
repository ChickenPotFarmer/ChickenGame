using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Status")]
    public bool movementActive;
    private bool cursorLocked;

    [Header("Target Image")]
    public Transform targetImage;

    [Header("Vectors Setup")]
    public Transform forward;
    public Transform fR;
    public Transform right;
    public Transform bR;
    public Transform back;
    public Transform bL;
    public Transform left;
    public Transform fL;

    private bool cursorWasLocked;
    private bool splatCannonActive;
    private Vector3 targetIconPos;
    private float prevMouseY;

    [Header("Setup")]
    public CinemachineFreeLook cmCam;
    public NavMeshAgent navAgent;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKey("w"))
        {
            if (Input.GetKey("d"))
                navAgent.SetDestination(fR.position);
            else if (Input.GetKey("a"))
                navAgent.SetDestination(fL.position);
            else
                navAgent.SetDestination(forward.position);

            movementActive = true;
        }

        else if (Input.GetKey("d"))
        {
            if (Input.GetKey("s"))
                navAgent.SetDestination(bR.position);
            else if (Input.GetKey("w"))
                navAgent.SetDestination(fR.position);
            else
                navAgent.SetDestination(right.position);

            movementActive = true;

        }

        else if (Input.GetKey("s"))
        {
            if (Input.GetKey("a"))
                navAgent.SetDestination(bL.position);
            else if (Input.GetKey("d"))
                navAgent.SetDestination(bR.position);
            else
                navAgent.SetDestination(back.position);

            movementActive = true;

        }

        else if (Input.GetKey("a"))
        {
            if (Input.GetKey("w"))
                navAgent.SetDestination(fL.position);
            else if (Input.GetKey("s"))
                navAgent.SetDestination(bL.position);
            else
                navAgent.SetDestination(left.position);

            movementActive = true;

        }
        else
        {
            if (movementActive)
            {
                navAgent.SetDestination(transform.position);
                movementActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(2))
        {
            ToggleCursor();

        }
        //else if (Input.GetMouseButtonDown(2))
        //{
        //    cursorWasLocked = cursorLocked;
        //    LockCursor();
        //}
        //else if (Input.GetMouseButtonUp(2))
        //{
        //    if (!cursorWasLocked)
        //        UnlockCursor();
        //}

        // Cursor lock and camera axis change
        //if (movementActive)
        //{
        //    if (!cursorLocked)
        //    {
        //        LockCursor();
        //        cmCam.m_XAxis.m_InputAxisName = "Mouse X";
        //    }
        //}
        //else
        //{
        //    if (cursorLocked)
        //    {
        //        UnlockCursor();
        //        cmCam.m_XAxis.m_InputAxisName = "QE Rotate";

        //    }
        //}

        if (splatCannonActive)
        {
            if (!targetImage.gameObject.activeInHierarchy)
                targetImage.gameObject.SetActive(true);

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
            //{
            //    targetImage.position = hit.point;
            //    targetImage.localPosition = new Vector3(0, 0, targetImage.localPosition.z);
            //}

            float diff = Input.mousePosition.y - prevMouseY;
            prevMouseY = Input.mousePosition.y;
            print(diff);

            targetImage.localPosition = new Vector3(0, 0, targetImage.localPosition.z - (diff / 2));
        }
        else
        {
            if (targetImage.gameObject.activeInHierarchy)
                targetImage.gameObject.SetActive(false);
        }

    }

    public void SplatCannonLock()
    {
        // only lock X, leave visable
        cursorLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        prevMouseY = Input.mousePosition.y;
        splatCannonActive = true;
        cmCam.m_XAxis.m_InputAxisName = "Mouse X";



    }

    public void SplatCannonUnlock()
    {
        splatCannonActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
        cmCam.m_XAxis.m_InputAxisName = "QE Rotate";


    }

    private void ToggleCursor()
    {
        if (cursorLocked && !splatCannonActive)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }

    }

    public void TempLock()
    {
        if (!splatCannonActive)
        {
            if (cursorWasLocked)
                LockCursor();
        }
    }

    public void TempUnlock()
    {
        if (!splatCannonActive)
        {
            cursorWasLocked = cursorLocked;
            UnlockCursor();
        }
    }

    public void LockCursor()
    {
        if (!splatCannonActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
            cmCam.m_XAxis.m_InputAxisName = "Mouse X";
        }
        
    }

    public void UnlockCursor()
    {
        if (!splatCannonActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLocked = false;
            cmCam.m_XAxis.m_InputAxisName = "QE Rotate";
        }
    }
}
