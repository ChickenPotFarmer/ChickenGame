using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggySearch : MonoBehaviour
{
    private ChickenController chickenController;
    private InputController inputController;

    private void Start()
    {
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!inputController)
            inputController = InputController.instance.inputController.GetComponent<InputController>();
    }
}
