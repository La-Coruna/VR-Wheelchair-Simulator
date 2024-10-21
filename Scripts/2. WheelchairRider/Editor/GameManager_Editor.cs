using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(GameManager))]
public class GameManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager g = (GameManager)target;

        if (GUILayout.Button("Allow Move"))
        {
            g.AllowMove();
        }
        if (GUILayout.Button("Prohibit Move"))
        {
            g.ProhibitMove();
        }
    }
}
