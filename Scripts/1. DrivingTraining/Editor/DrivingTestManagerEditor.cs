using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(DrivingTestManager))]
public class DrivingTestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        DrivingTestManager drivingTestManager = (DrivingTestManager)target;

        if (GUILayout.Button("Start Driving Test"))
        {
            drivingTestManager.StartTest();
        }
        if (GUILayout.Button("Force Stop"))
        {
            drivingTestManager.EndTest();
        }    

        if (GUILayout.Button("Open Folder"))
        {
            drivingTestManager.OpenFolder();
        }
    }
}
