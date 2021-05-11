using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StrainProfile))]
public class StrainProfileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StrainProfile myScript = (StrainProfile)target;

        GUILayout.Label("Strain", EditorStyles.miniLabel);
        GUILayout.Label(myScript.strainName, EditorStyles.largeLabel);

        GUILayout.Space(5f); //2

        GUILayout.Label("Type", EditorStyles.miniLabel);
        GUILayout.Label(myScript.GetStrainType(), EditorStyles.largeLabel);

        GUILayout.Space(5f); //2

        GUILayout.Label("THC Content", EditorStyles.miniLabel);
        GUILayout.Label(myScript.GetReaderFriendlyThcContent(), EditorStyles.largeLabel);

        GUILayout.Space(5f); //2

        GUILayout.Label("Terpene Effects", EditorStyles.miniLabel);
        GUILayout.Label("Primary: \t\t" + myScript.primaryEffect, EditorStyles.largeLabel);
        GUILayout.Label("Secondary: \t" + myScript.secondaryEffect, EditorStyles.largeLabel);
        //GUILayout.Label("Lesser: \t\t" + myScript.GetLesserTerpene(), EditorStyles.largeLabel);

        GUILayout.Space(5f); //2

        GUILayout.Label("Terpenes", EditorStyles.miniLabel);
        GUILayout.Label("Primary: \t\t" + myScript.GetPrimaryTerpene(), EditorStyles.largeLabel);
        GUILayout.Label("Secondary: \t" + myScript.GetSecondaryTerpene(), EditorStyles.largeLabel);
        GUILayout.Label("Lesser: \t\t" + myScript.GetLesserTerpene(), EditorStyles.largeLabel);

        GUILayout.Space(5f); //2

        GUILayout.Label("Terpene Levels", EditorStyles.miniLabel);
        GUILayout.Label("Total Terpene Content: \t" + (myScript.totalTerpenesPercent * 100).ToString("n2") + "%", EditorStyles.largeLabel);
        GUILayout.Label("Caryophyllene: \t" + myScript.GetReaderFriendlyTerpeneLvl(0), EditorStyles.largeLabel);
        GUILayout.Label("Limonene: \t" + myScript.GetReaderFriendlyTerpeneLvl(1), EditorStyles.largeLabel);
        GUILayout.Label("Linalool: \t\t" + myScript.GetReaderFriendlyTerpeneLvl(2), EditorStyles.largeLabel);
        GUILayout.Label("Myrcene: \t\t" + myScript.GetReaderFriendlyTerpeneLvl(3), EditorStyles.largeLabel);
        GUILayout.Label("Pinene: \t\t" + myScript.GetReaderFriendlyTerpeneLvl(4), EditorStyles.largeLabel);
        GUILayout.Label("Terpinolene: \t" + myScript.GetReaderFriendlyTerpeneLvl(5), EditorStyles.largeLabel);


        if (GUILayout.Button("Generate Terpene Levels"))
        {
            myScript.GenerateTerpeneLevels();
        }

        if (GUILayout.Button("Reset Terpene Levels"))
        {
            myScript.ResetTerpeneLevels();
        }

        DrawDefaultInspector();
    }
}

