using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Serialization;

public class ReplayManager_v2 : MonoBehaviour
{
    public GameObject _wheelchairModel;
    List<Tuple<float, float, float, float, float, float, float>> _pathRecord;

    public string _name;
    public int _trial;

    private Coroutine _replayCoroutine;
    private bool _isReplayDone = false;

    // Ranking.BestPlayerInfo로 name과 trial을 알아와서 ReadCSVByNameTrial 실행.
    public void ReadCSV_BestRecord()
    {
        var bestPlayer = Ranking.Instance.BestPlayerInfo;

        if (bestPlayer != null)
        {
            ReadCSVByNameTrial(bestPlayer.Item1, bestPlayer.Item2);
        }
        else
        {
            Debug.LogWarning("랭킹 1위 기록을 찾을 수 없습니다.");
        }
    }

    // 이름과 시도 횟수로 csv로 저장된 기록파일을 _pathRecord를 불러옴.
    public void ReadCSVByNameTrial(string name, int trial)
    {
        _pathRecord = new List<Tuple<float, float, float, float, float, float, float>>();

        // 파일 경로 생성
        string filePath = Path.Combine(PathConfig.FolderPath, $"{name}_{trial}.csv");

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // 첫 번째 줄 (TotalTime) 건너뛰기
                reader.ReadLine(); // 두 번째 줄 (헤더) 건너뛰기

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    if (values.Length == 7)
                    {
                        float time = float.Parse(values[0]);
                        float posX = float.Parse(values[1]);
                        float posY = float.Parse(values[2]);
                        float posZ = float.Parse(values[3]);
                        float rotX = float.Parse(values[4]);
                        float rotY = float.Parse(values[5]);
                        float rotZ = float.Parse(values[6]);

                        _pathRecord.Add(new Tuple<float, float, float, float, float, float, float>(
                            time, posX, posY, posZ, rotX, rotY, rotZ));
                    }
                }
            }

            Debug.Log("기록이 성공적으로 로드되었습니다: " + filePath);
        }
        else
        {
            Debug.LogError("기록 파일을 찾을 수 없습니다: " + filePath);
        }
    }

    // 가져온 _pathRecord로 리플레이 시작.
    // _pathRecord가 없으면 시작 x
    public void StartReplay()
    {
        if (_pathRecord == null || _pathRecord.Count == 0)
        {
            Debug.LogWarning("리플레이할 기록이 없습니다.");
            return;
        }
        Debug.LogWarning("리플레이 시작.");
        _replayCoroutine = StartCoroutine(ReplayCoroutine());
    }

    public void StartReplayWithTop1()
    {
        ReadCSV_BestRecord();
        StartReplay();
    }

    // 리플레이 코루틴
    private IEnumerator ReplayCoroutine()
    {
        float prevTime = 0f;
        foreach (var record in _pathRecord)
        {
            _wheelchairModel.transform.position = new Vector3(record.Item2, record.Item3, record.Item4);
            _wheelchairModel.transform.eulerAngles = new Vector3(record.Item5, record.Item6, record.Item7);

            //Debug.Log($"리플레이중입니다. {_wheelchairModel.transform.position.ToString()}");
            float waitTime = record.Item1 - prevTime;
            prevTime = record.Item1;
            yield return new WaitForSeconds(waitTime);
        }
        _isReplayDone = true;
        Debug.Log("리플레이가 완료되었습니다.");
    }

    public void StopReplay()
    {
        if (_replayCoroutine != null)
        {
            StopCoroutine(_replayCoroutine);
            _replayCoroutine = null;
            Debug.Log("리플레이가 중지되었습니다.");
        }
    }
    public bool IsReplayDone()
    {
        return _isReplayDone;
    }
}
