using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : MonoBehaviour
{
    [Header("Interactable Items")]
    public Atm atm;
    public DryerController dryer;

    [Header("Visual Indicators")]
    public GameObject keyboardKeyIcon;

    // set up interaction manager that will make sure only one selection at a time

    public void SetInteractable(bool _canInteract)
    {
        if (_canInteract)
        {
            if (atm)
                atm.SetInteractable(true);
            else if (dryer)
                dryer.SetInteractable(true);

            keyboardKeyIcon.SetActive(true);

        }
        else
        {
            if (atm)
                atm.SetInteractable(false);
            else if (dryer)
                dryer.SetInteractable(false);

            keyboardKeyIcon.SetActive(false);

        }
    }
}
