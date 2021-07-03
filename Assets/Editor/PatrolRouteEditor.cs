using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[CustomEditor(typeof(PatrolRoute))]

public class PatrolRouteEditor : Editor
{
    private bool routineStarted;
    private bool scriptSelected;
    public override void OnInspectorGUI()
    {
        scriptSelected = true;
        
        if (!routineStarted)
            EditorCoroutineUtility.StartCoroutine(DrawLineRoutine(), this);
        //if (GUILayout.Button("Update Line"))
        //{
        //    myScript.ResetTerpeneLevels();
        //}

        DrawDefaultInspector();

        
    }

    private void OnDisable()
    {
        PatrolRoute myScript = (PatrolRoute)target;

        scriptSelected = false;
        //myScript.lineRenderer.enabled = false;

    }

    IEnumerator DrawLineRoutine()
    {
        PatrolRoute myScript = (PatrolRoute)target;
        routineStarted = true;
        myScript.lineRenderer.enabled = true;
        while (myScript.updateLine && scriptSelected)
        {
            if (myScript.waypointsParent)
            {
                Vector3[] points = new Vector3[myScript.waypointsParent.childCount];

                for (int i = 0; i < points.Length; i++)
                {
                    Vector3 temp = new Vector3(myScript.waypointsParent.GetChild(i).position.x, 1, myScript.waypointsParent.GetChild(i).position.z);
                    points[i] = temp;
                }

                myScript.lineRenderer.positionCount = points.Length;
                myScript.lineRenderer.SetPositions(points);
            }
            yield return new WaitForSeconds(0.1f);
        }
        routineStarted = false;
    }
}
