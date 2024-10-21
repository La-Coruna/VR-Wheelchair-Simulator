using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(PathRecorder_v2))]
public class PathRecorder_v2_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        PathRecorder_v2 pathRecorder_v2 = (PathRecorder_v2)target;

            if (GUILayout.Button("Start Record"))
            {
                pathRecorder_v2.StartRecord();
            }
            if (GUILayout.Button("End Record"))
            {
                pathRecorder_v2.EndRecord();
            }
            if (GUILayout.Button("Open Record Folder"))
            {
                pathRecorder_v2.OpenFilePath();
            }

    }
}
