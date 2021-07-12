using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Status")]
    public bool movementActive;
    private bool cursorLocked;

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

    [Header("Setup")]
    public CinemachineFreeLook cmCam;
    public NavMeshAgent navAgent;

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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCursor();

        }
        else if (Input.GetMouseButtonDown(2))
        {
            cursorWasLocked = cursorLocked;
            LockCursor();
        }
        else if (Input.GetMouseButtonUp(2))
        {
            if (!cursorWasLocked)
                UnlockCursor();
        }

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

    }

    private void ToggleCursor()
    {
        if (cursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }

    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
        cmCam.m_XAxis.m_InputAxisName = "Mouse X";
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
        cmCam.m_XAxis.m_InputAxisName = "QE Rotate";
    }
}
