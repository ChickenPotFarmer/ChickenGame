using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Alerts
{
    private static AlertDisplay alertDisplay;

    public static void DisplayMessage(string _message)
    {
        if (!alertDisplay)
            alertDisplay = AlertDisplay.instance.alerts.GetComponent<AlertDisplay>();

        alertDisplay.DisplayMessage(_message);
    }
}
