using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToDoObject : MonoBehaviour
{
    public Text toDoTxt;

    public void Complete()
    {
        //do something cooler than this
        Destroy(gameObject);
    }
}
