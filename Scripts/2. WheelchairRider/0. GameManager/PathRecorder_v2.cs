using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class PathRecorder_v2 : MonoBehaviour
{
    private bool _canStarted = true;
    private bool _isRecording = false;
    public Transform _wheelchair;
    
#region Variables for Recording
    // 기록할 때마다 항상 초기화 해줘야 함.

    // Record File Nmae
    public string _name = "name";
    public int _trial = 1;

    // Recorded Variable
    public float _currentTime = 0f;
    // current Time, position x,y,z, rotation x,y,z
    public List<Tuple<float, float, float, float, float, float, float>> _pathRecord;
    #endregion

    //private void InitRecord(string name)
    private void InitRecord()
    {
        _name = PlayerPrefs.GetString("Name");

        _currentTime = 0f;
        _pathRecord = new List<Tuple<float, float, float, float, float, float, float>>();
    }

    public void StartRecord()
    {
        // 시작할 수 없는 상태면 그냥 빠져나오기.
        if (!_canStarted) return;

        _canStarted = false;
        InitRecord(); // 나중에는 플레이할 때 입력할 수 있도록.

        _isRecording = true;
    }

    public void EndRecord()
    {
        _isRecording = false;
        WriteRecordCSV();

        PathDrawer.createImageWithPathRecord(_pathRecord, new List<Vector3>(), "PathImage.png");

        Debug.Log("랭킹 작성 호출.");
        Ranking.Instance.UpdateRanking(_currentTime, _name, _trial);
        Ranking.Instance.WriteRankingCSV();

        _canStarted = true;
    }
    
    private void RecordPath()
    {
        Vector3 wheelchairPosition = _wheelchair.position;
        Vector3 wheelchairRotation = _wheelchair.eulerAngles;
        _currentTime += Time.deltaTime;
        _pathRecord.Add(new Tuple<float, float, float, float, float, float, float>(
            _currentTime,
            wheelchairPosition.x, wheelchairPosition.y, wheelchairPosition.z,
            wheelchairRotation.x, wheelchairRotation.y, wheelchairRotation.z
        ));
    }

    private void WriteRecordCSV()
    {
        _trial = 1;

        // 폴더 이름 설정, 없으으면 생성
        if (!Directory.Exists(PathConfig.FolderPath))
        {
            Directory.CreateDirectory(PathConfig.FolderPath);
        }
        
        // 같은 name을 가진 csv파일이 있으면 trial 증가.
        string filePath;
        filePath = Path.Combine(PathConfig.FolderPath, _name + "_" + _trial + ".csv");

        while (File.Exists(filePath))
        {
            filePath = Path.Combine(PathConfig.FolderPath, _name + "_" + ++_trial + ".csv");
        }
        
        // 파일 열고 작성 시작.
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // 최종 시간과 완주 여부 기록
            string firstLine = $"TotalTime,{_currentTime}";
            writer.WriteLine(firstLine);
        
            // 기록된 데이터를 CSV 파일에 작성
            writer.WriteLine("current time,position x,position y,position z,rotation x,rotation y,rotation z");
            foreach (var record in _pathRecord)
            {
                string line = $"{record.Item1},{record.Item2},{record.Item3},{record.Item4},{record.Item5},{record.Item6},{record.Item7}";
                writer.WriteLine(line);
            }
        }

        Debug.Log("CSV 파일이 작성되었습니다: " + filePath);
    }

    void Start()
    {
    }

    void Update()
    {
        if (_isRecording) RecordPath();
    }

    public void OpenFilePath()
    {
        // 저장된 파일이 있는 경로를 연다
        string folderPath = PathConfig.FolderPath;

        if (Directory.Exists(folderPath))
        {
            // OS에 따라 다른 명령 사용
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", folderPath);
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                System.Diagnostics.Process.Start("open", folderPath);
#else
                Debug.LogWarning("이 플랫폼에서는 파일 경로 열기가 지원되지 않습니다.");
#endif
        }
        else
        {
            Debug.LogWarning("파일 경로가 존재하지 않습니다: " + folderPath);
        }
    }
}
