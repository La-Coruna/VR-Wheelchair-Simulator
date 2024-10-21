using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(ReplayManager_v2))]
public class ReplayManager_v2_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        ReplayManager_v2 r = (ReplayManager_v2)target;

            if (GUILayout.Button("Load By Name & Trial"))
            {
                r.ReadCSVByNameTrial(r._name, r._trial);
            }
            if (GUILayout.Button("Load BestPlayer"))
            {
                r.ReadCSV_BestRecord();
            }
            if (GUILayout.Button("Replay Start"))
            {
                r.StartReplay();
            }
    }
}
