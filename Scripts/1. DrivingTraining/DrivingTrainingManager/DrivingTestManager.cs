using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrivingTestManager : MonoBehaviour
{
    public static DrivingTestManager instance;
    
    public PathRecorder pathRecorder;  // 이동 경로를 CSV로 기록해줌
    
    public int _participantsNumber;
    //public int trialNum = 0;
    public Camera _camera;
    public Transform _wheelchair;
    public float endTime = 600.0f;
    
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        
        instance = this;
    }
    
    void Start()
    {
        pathRecorder = new PathRecorder(_participantsNumber,_camera,_wheelchair,endTime);
    }

    void Update()
    {
        pathRecorder.RecordPath();
    }

    public void StartTest()
    {
        pathRecorder.CreateCSVFile();
        pathRecorder.Start_Timer();
    }

    public void EndTest()
    {
        pathRecorder.End_Timer();
        pathRecorder.closeCSVFile();
    }
    
    public void WriteCollision()
    {
        pathRecorder.SaveCollisionPositionsToCSV();
    }

    public void OpenFolder()
    {
        pathRecorder.OpenFolder();
    }
    
}
