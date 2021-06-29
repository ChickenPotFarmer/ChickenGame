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
            //navAgent.SetDestination(transform.position);
            movementActive = false;
        }

        // Cursor lock and camera axis change
        if (movementActive)
        {
            if (!cursorLocked)
            {
                cursorLocked = true;
                Cursor.lockState = CursorLockMode.Locked;
                cmCam.m_XAxis.m_InputAxisName = "Mouse X";
            }
        }
        else
        {
            if (cursorLocked)
            {
                cursorLocked = false;
                Cursor.lockState = CursorLockMode.None;
                cmCam.m_XAxis.m_InputAxisName = "QE Rotate";

            }
        }

    }
}
