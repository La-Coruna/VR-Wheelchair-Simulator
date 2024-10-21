using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Serialization;

public class ReplayManager : MonoBehaviour
{

    private List<ReplayData> replayDataList; // replay 데이터를 저장할 리스트
    private List<CollisionData> collisionDataList; // 충돌한 위치
    private int currentIndex = 0; // 현재 인덱스
    private bool isReplaying = false; // replay 중인지 여부
    private bool isLoaded = false;
    public bool IsLoaded => isLoaded;

    // 프로퍼티
    public int CurrentIndex => currentIndex;
    public bool IsReplaying => isReplaying;

    public int participantsNumber = 0;
    public int trialNum = 1;

    public GameObject replayModeWheelchair; // replay 할 GameObject
    public GameObject replayModeCamera;
    public GameObject playModeWheelchair;
    
    public GameObject collisionTracePrefab;
    private List<GameObject> spawnedCollisionTraces = new List<GameObject>(); // 생성된 프리팹들을 저장하기 위한 리스트

    [HideInInspector] public bool isSpawned = false;

    
    //실험 폴더 경로
    private string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/DS_Study_DrivingTest/";

    void Start()
    {
    }

    int ReadAllData()
    {
        int result = ReadMovementCSV() + ReadCollisionCSV(); // 성공했으면 0 하나라도 실패시 1
        return result;
    }
    
    
    public void TryLoadData()
    {
        isLoaded = ReadAllData() == 0; // 성공했으면 0이므로 not 취해줘서 true
    }

    // CSV 파일 읽기
    private int ReadMovementCSV()
    {
        // 실험 폴더 경로 + csv 파일 경로
        string csvFilePath = path + participantsNumber + "번 피험자/" + trialNum + "/WheelchairMovement.csv";
        
        replayDataList = new List<ReplayData>();
        
        // 파일 존재 여부 확인
        if (!File.Exists(csvFilePath))
        {
            Debug.Log("CSV 파일이 존재하지 않습니다: " + csvFilePath);
            return 1; // 파일이 없으므로 1을 반환
        }

        // CSV 파일 읽기
        string[] lines = File.ReadAllLines(csvFilePath);

        // 첫 번째 라인은 헤더이므로 무시하고 두 번째 라인부터 데이터를 읽음
        for (int i = 1; i < lines.Length -1; i++)
        {
            string[] values = lines[i].Split(',');
            ReplayData replayData = new ReplayData();
            replayData.CurrentTime = float.Parse(values[0]);
            replayData.Position = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            replayData.Rotation = new Vector3(float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]));
            replayDataList.Add(replayData);
        }
        
        return 0; // 파일을 성공적으로 읽었으므로 0을 반환
    }
    
    private int ReadCollisionCSV()
    {
        // 실험 폴더 경로 + csv 파일 경로
        string csvFilePath = path + participantsNumber + "번 피험자/" + trialNum + "/Collision.csv";
        
        collisionDataList = new List<CollisionData>();

        // 파일 존재 여부 확인
        if (!File.Exists(csvFilePath))
        {
            Debug.Log("CSV 파일이 존재하지 않습니다: " + csvFilePath);
            return 1; // 파일이 없으므로 1을 반환
        }
        
        // CSV 파일 읽기
        string[] lines = File.ReadAllLines(csvFilePath);

        // 첫 번째 라인은 헤더이므로 무시하고 두 번째 라인부터 데이터를 읽음
        for (int i = 1; i < lines.Length -1; i++)
        {
            string[] values = lines[i].Split(',');
            CollisionData collisionData = new CollisionData();
            collisionData.CurrentTime = float.Parse(values[0]);
            collisionData.Position = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            collisionDataList.Add(collisionData);
        }
        
        return 0; // 파일을 성공적으로 읽었으므로 0을 반환
    }

    public void SpawnCollisionTracePrefabs()
    {
        // 리스트에 저장된 각 위치에 프리팹을 생성합니다.
        isSpawned = true;
        foreach (CollisionData collisionData in collisionDataList)
        {
            GameObject newPrefab = Instantiate(collisionTracePrefab, collisionData.Position, Quaternion.identity, transform);
            spawnedCollisionTraces.Add(newPrefab);
        }
    }
    
    public void ClearPrefabs()
    {
        isSpawned = false;

        // 생성된 모든 프리팹들을 삭제합니다.
        foreach (GameObject prefab in spawnedCollisionTraces)
        {
            Destroy(prefab);
        }
        spawnedCollisionTraces.Clear(); // 리스트 초기화
    }

    public void ToggleCollisionTracePrefabs()
    {
        // 생성된 모든 프리팹들의 활성/비활성 상태를 반전시킵니다.
        foreach (GameObject prefab in spawnedCollisionTraces)
        {
            prefab.SetActive(!prefab.activeSelf);
        }
    }
    
    void OnDestroy()
    {
        // 스크립트가 삭제될 때 생성된 모든 프리팹을 삭제합니다.
        ClearPrefabs();
    }
    
    // Replay 하려는 CSV가 달라진 경우 감지
    private void OnValidate()
    {
        isReplaying = false;
        currentIndex = 0;
        ClearPrefabs();
        isLoaded = false;
    }
    
    // Replay 시 카메라 전환 및 휠체어 오브젝트 변경
    public void PrepareReplay()
    {
        playModeWheelchair.SetActive(false);
        replayModeWheelchair.SetActive(true);
        replayModeCamera.SetActive(true);
        return;
    }
    
    // Replay 종료 시 카메라 전환 및 휠체어 오브젝트 변경
    public void WrapUpReplay()
    {
        playModeWheelchair.SetActive(true);
        replayModeWheelchair.SetActive(false);
        replayModeCamera.SetActive(false);
        return;
    }

    
    // Replay 시작
    public void StartReplay()
    {
        if (playModeWheelchair.activeSelf || !replayModeWheelchair.activeSelf || !replayModeWheelchair)
        {
            PrepareReplay();
        }
        isReplaying = true;
        StartCoroutine(ReplayCoroutine());
    }

    // Replay 일시 중지
    public void PauseReplay()
    {
        isReplaying = false;
    }
    
    // Replay Coroutine
    private IEnumerator ReplayCoroutine()
    {
        float startTime = Time.time; // replay 시작 시간 저장

        // resume할 시, currentIndex의 시간이 될 때까지 안 기다리도록.
        if (currentIndex > 0)
            startTime -= replayDataList[currentIndex].CurrentTime;
        
        // currentIndex가 replayDataList의 범위 내에 있고 isReplaying이 true인 동안 반복
        while (isReplaying)
        {
            if (currentIndex < replayDataList.Count)
            {
                ReplayData replayData = replayDataList[currentIndex];
                float elapsedTime = Time.time - startTime; // 현재까지의 경과 시간 계산

                // 현재 경과 시간이 replayData의 시간에 도달할 때까지 기다림
                while (elapsedTime < replayData.CurrentTime)
                {
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }

                // replayModeWheelchair의 위치와 회전을 설정
                replayModeWheelchair.transform.position = replayData.Position;
                replayModeWheelchair.transform.eulerAngles = replayData.Rotation;

                currentIndex++;
                
            }
            else
            {
                currentIndex = 0;
                break;
            }
        }
    }
    
    public void OpenFolder()
    {
        System.Diagnostics.Process.Start( System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/DS_Study_DrivingTest/" + participantsNumber + "번 피험자/" + trialNum + "/");
    }

}

public class CollisionData
{
    public float CurrentTime;
    public Vector3 Position;
}

// Replay 데이터를 담는 클래스
public class ReplayData
{
    public float CurrentTime;
    public Vector3 Position;
    public Vector3 Rotation;
}