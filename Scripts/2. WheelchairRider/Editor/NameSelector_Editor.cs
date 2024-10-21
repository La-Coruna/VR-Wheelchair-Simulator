using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(NameSelector))]
public class NameSelector_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NameSelector t = (NameSelector)target;

        //-----------------------------------------------------
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("PrevAlpha"))
        {
            t.PrevAlpha();
        }
        if (GUILayout.Button("NextAlpha"))
        {
            t.NextAlpha();
        }
        GUILayout.EndHorizontal();

        //-----------------------------------------------------
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("PrevCursor"))
        {
            t.PrevCursor();
        }
        if (GUILayout.Button("NextCursor"))
        {
            t.NextCursor();
        }
        GUILayout.EndHorizontal();

    }
}
