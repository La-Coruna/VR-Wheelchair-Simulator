using UnityEngine;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(ReplayManager))]
public class ReplayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ReplayManager replayManager = (ReplayManager)target;

        if (replayManager.IsLoaded)
        {
            // replay중이 아닐 때
            if (!replayManager.IsReplaying)
            {
                // start or resume replay
                if (replayManager.CurrentIndex == 0)
                {
                    if (GUILayout.Button("Start Replay"))
                    {
                        replayManager.StartReplay();
                    }
                }
                else
                {
                    if (GUILayout.Button("Resume Replay"))
                    {
                        replayManager.StartReplay();
                    }
                }

                // switch camera and object
                if (!replayManager.playModeWheelchair.activeSelf)
                {
                    if (GUILayout.Button("Switch to Play Mode"))
                    {
                        replayManager.WrapUpReplay();
                    }
                }
                else
                {
                    if (GUILayout.Button("Switch to Replay Mode"))
                    {
                        replayManager.PrepareReplay();
                    }
                }
            }
            // replay 중일 때
            else
            {
                if (replayManager.IsReplaying)
                {
                    if (GUILayout.Button("Pause Replay"))
                    {
                        replayManager.PauseReplay();
                    }
                }
            }

            if (!replayManager.isSpawned)
            {
                if (GUILayout.Button("Show Collision Trace"))
                {
                    replayManager.SpawnCollisionTracePrefabs();
                }
            }
            else
            {
                if (GUILayout.Button("Clear Collision Trace"))
                {
                    replayManager.ClearPrefabs();
                }
            }

            if (GUILayout.Button("Open Folder"))
            {
                replayManager.OpenFolder();
            }
        }
        else // data가 없는 경우
        {
            if (GUILayout.Button("Load Data"))
            {
                replayManager.TryLoadData();
            }
        }
    }
}