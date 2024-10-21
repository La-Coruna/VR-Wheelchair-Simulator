using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(TrainingAudioPlayer))]
public class TrainingAudioPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrainingAudioPlayer editor = (TrainingAudioPlayer)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Prev"))
        {
            editor.PlayPreviousSound();
        }
        
        if (GUILayout.Button("PlaySound"))
        {
            editor.PlaySound();
        }

        if (GUILayout.Button("Next"))
        {
            editor.PlayNextSound();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("GoodJob"))
        {
            editor.PlayGodJobSound();
        }
    }
}