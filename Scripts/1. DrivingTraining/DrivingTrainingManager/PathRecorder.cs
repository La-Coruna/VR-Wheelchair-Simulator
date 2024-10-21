using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


[Serializable()]
public class PathRecorder
{
    // 디렉토리 저장 경로
    static string path;
    
    public int _participantsNumber;
    public static int participantsNumber;
    public int trialNum = 0;

    public Camera _camera;
    public Transform _wheelchair;

    public float endTime = 600.0f;
    
    static CsvFileWriter HeadMovement;
    static List<string> HeadColumns;

    static CsvFileWriter WheelchairMovement;
    static List<string> WheelchairColumns;

    static CsvFileWriter CollisionNum;
    static List<string> CollisionColumns;

    public static bool isEnded = true;

    public static float currentTime = 0f;

    // PATH DRAWER
    private static List<Vector3> pathPoints = new List<Vector3>(); // 그림 그릴 점들

    public List<float> collisionTimes = new List<float>();
    public int collisionIdx = 0;
    

    public PathRecorder(int _participantsNumber, Camera _camera, Transform _wheelchair, float endTime)
    {
        this._participantsNumber = _participantsNumber;
        this._camera = _camera;
        this._wheelchair = _wheelchair;
        this.endTime = endTime;

        CollisionLogger.OnWheelchairCollision += wheelchairCollisionHandler;
    }
    
    public void CreateCSVFile()
    {
        participantsNumber = _participantsNumber;


        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
            "/DS_Study_DrivingTest/" + participantsNumber + "번 피험자/";

        // 폴더 유무 확인
        DirectoryInfo di = new DirectoryInfo(path);

        while (di.Exists)    // ex) trial 1 폴더가 이미 있으면 trial 2 폴더를 생성하게끔 설정 (1, 2 존재 -> 3 생성)
        {
            ++trialNum;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
                "/DS_Study_DrivingTest/" + participantsNumber + "번 피험자/" + trialNum + "/";
            di = new DirectoryInfo(path);
        }

        // 폴더가 없으면 폴더 생성
        if (!di.Exists)
            di.Create();



        
        HeadMovement = new CsvFileWriter(path + "HeadMovement.csv");
        HeadColumns = new List<string>() { "CurrentTime", "HeadRot_x", "HeadRot_y", "HeadRot_z" };
        WheelchairMovement = new CsvFileWriter(path + "WheelchairMovement.csv");
        WheelchairColumns = new List<string>() { "CurrentTime", "WCPos_x", "WCPos_y", "WCPos_z", "WCRot_x", "WCRot_y", "WCRot_z" };
        
        HeadMovement.WriteRow(HeadColumns);
        HeadColumns.Clear();
        WheelchairMovement.WriteRow(WheelchairColumns);
        WheelchairColumns.Clear();
    }

    // isEnded가 false면 recording.
    public void RecordPath()
    {
        if (!isEnded)
        {
            Check_Timer();
            WritingHeadData();
            WritingWheelchairData();
            
            if (currentTime > endTime)
            {
                End_Timer();
            }
        }
    }
    
    // Timer 설정
    private void Check_Timer()
    {
        currentTime += Time.deltaTime;
    }

    public void Start_Timer()
    {
        isEnded = false;
        Debug.Log("Start");
    }

    public void End_Timer()
    {
        Debug.Log("End. Time is " + currentTime);
        isEnded = true;

        // collision 위치 기록.
        SaveCollisionPositionsToCSV();
        
        // 이동 경로 그리기
        PathDrawer.createImageWithPathPoints(pathPoints, CollisionLogger.CollisionPositions ,path + "PathImage.png");
        
        //게임 강제 종료.
        StaticCoroutine.DoCoroutine(GameClearQuit());
    }
    
    void WritingHeadData()
    {
        // Head의 Rotation 기록 (Head rotation 값은 inspector 값 그대로 가져옴)
        var headRotation = _camera.transform.rotation.eulerAngles;

        HeadColumns.Add(currentTime.ToString());
        HeadColumns.Add(headRotation.x.ToString());
        HeadColumns.Add(headRotation.y.ToString());
        HeadColumns.Add(headRotation.z.ToString());

        HeadMovement.WriteRow(HeadColumns);
        HeadColumns.Clear();
    }

    void WritingWheelchairData()
    {
        // Head의 Rotation 기록 (Head rotation 값은 inspector 값 그대로 가져옴)
        Vector3 wheelchairPosition = _wheelchair.position;
        Vector3 wheelchairRotation = _wheelchair.rotation.eulerAngles;

        WheelchairColumns.Add(currentTime.ToString());
        WheelchairColumns.Add(wheelchairPosition.x.ToString());
        WheelchairColumns.Add(wheelchairPosition.y.ToString());
        WheelchairColumns.Add(wheelchairPosition.z.ToString());
        WheelchairColumns.Add(wheelchairRotation.x.ToString());
        WheelchairColumns.Add(wheelchairRotation.y.ToString());
        WheelchairColumns.Add(wheelchairRotation.z.ToString());

        WheelchairMovement.WriteRow(WheelchairColumns);
        WheelchairColumns.Clear();
        
        //To draw path
        pathPoints.Add( new Vector3(wheelchairPosition.x, wheelchairPosition.y, wheelchairPosition.z) );
    }

    public void wheelchairCollisionHandler(Vector3 collisionPoint)
    {
        if (!isEnded)
        {
            collisionTimes.Add(currentTime);
            //Debug.Log("충돌발생 이벤트 받음.");
        }
    }  

    private void OnApplicationQuit()
    {
        closeCSVFile();
    }

    public void closeCSVFile()
    {
        HeadMovement?.Dispose();
        WheelchairMovement?.Dispose();
        CollisionNum?.Dispose();
    }
    
    public void SaveCollisionPositionsToCSV()
    {
        string filePath = path + "Collision.csv";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            int collisionTimeIdx = 0;
            writer.WriteLine("CurrentTime,x,y,z");
            foreach (Vector3 position in CollisionLogger.CollisionPositions)
            {
                //writer.WriteLine(collisionTimes[collisionTimeIdx++] + "," + position.x + "," + position.y + "," + position.z); // 왜 에러?
            }
        }

        Debug.Log("Collision positions saved to CSV: " + filePath);
    }
    
    public void OpenFolder()
    {
        System.Diagnostics.Process.Start( System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/DS_Study_DrivingTest/" + participantsNumber + "번 피험자/" + trialNum + "/");
    }

    // 5초 후 종료 코루틴
    public static IEnumerator GameClearQuit()
    {
        yield return new WaitForSecondsRealtime(5f);
        GameQuit();
    }

    // 게임 종료
    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
