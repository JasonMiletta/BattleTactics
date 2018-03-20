using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MatchManager))]
public class MatchManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        MatchManager matchManagerScript = (MatchManager)target;
        if(GUILayout.Button("Calculate Teams"))
        {
            matchManagerScript.initializeTeams();
        }
    }
}