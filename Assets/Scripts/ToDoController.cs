using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoController : MonoBehaviour
{
    [Header("Panel")]
    public Animator panelAnimator;
    public bool isOpen;

    [Header("To-Do's")]
    public Transform toDoParent;
    public GameObject toDoPrefab;

    public static ToDoController instance;
    [HideInInspector]
    public GameObject toDoController;

    private void Awake()
    {
        instance = this;
        toDoController = gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleToDoPanel();
    }

    public void ToggleToDoPanel()
    {
        if (isOpen)
        {
            panelAnimator.Play("Close");
            isOpen = false;
        }
        else
        {
            panelAnimator.Play("Open");
            isOpen = true;
        }
    }

    public ToDoObject AddOrderToDo(float _amt, string _name)
    {
        GameObject newToDo = Instantiate(toDoPrefab, toDoParent);
        string toDoText = "Deliver " + _amt.ToString("n1") + "g to " + _name;
        ToDoObject toDo = newToDo.GetComponent<ToDoObject>();
        toDo.toDoTxt.text = toDoText;

        return toDo;
    }
}
