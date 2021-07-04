using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ATM":
            case "Dryer":
                OpenTarget(other);
                break;

            default:
                print("no tag found");
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "ATM":
            case "Dryer":
                CloseTarget(other);
                break;

            default:
                print("no tag found");
                break;
        }
    }

    private void OpenTarget(Collider _collder)
    {
        InteractWith target = _collder.GetComponent<InteractWith>();

        target.SetInteractable(true);
    }

    private void CloseTarget(Collider _collder)
    {
        InteractWith target = _collder.GetComponent<InteractWith>();

        target.SetInteractable(false);
    }
}
