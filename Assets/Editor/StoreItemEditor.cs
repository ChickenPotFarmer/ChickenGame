using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FarmBuilderItem))]

public class StoreItemEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FarmBuilderItem myScript = (FarmBuilderItem)target;
        myScript.titleTxt.text = myScript.itemName;
        myScript.costTxt.text = "$" + myScript.storeCost.ToString("n0");
    }

}
