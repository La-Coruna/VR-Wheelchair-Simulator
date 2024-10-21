using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Ranking
{
    private static Ranking _instance = null;
    private static readonly object _lock = new object();
    public static Ranking Instance
    {
        get
        {
            // 안전한 싱글톤을 위해.
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Ranking();
                    _instance.Init();
                }
            }
            return _instance;
        }
    }
    
    public bool _hasRanking = false;
    
    // totalTime, name, trial
    private List<Tuple<float, string, int>> _ranking = new List<Tuple<float, string, int>>();
    
    public IReadOnlyList<Tuple<float, string, int>> RankingList
    {
        get { return _ranking.AsReadOnly(); }
    }
    
    // 1위 플레이어의 이름과 trial을 반환하는 프로퍼티
    public Tuple<string, int> BestPlayerInfo
    {
        get
        {
            if (_ranking.Count > 0)
            {
                var topPlayer = _ranking[0];
                return new Tuple<string, int>(topPlayer.Item2, topPlayer.Item3);
            }
            return null; // 랭킹이 없으면 null 반환
        }
    }
    

    public void Init()
    {
        ReadRankingCSV();
    }
    
    public void ReadRankingCSV()
    {
        string rankingFilePath = Path.Combine(PathConfig.FolderPath, PathConfig.RankingFileName);

        if (File.Exists(rankingFilePath))
        {
            using (StreamReader reader = new StreamReader(rankingFilePath))
            {
                reader.ReadLine(); // Skip header

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    if (values.Length == 3 && float.TryParse(values[0], out float time) && int.TryParse(values[2], out int trial))
                    {
                        _ranking.Add(new Tuple<float, string, int>(time, values[1], trial));
                    }
                }
            }

            _hasRanking = true;
            Debug.Log("랭킹 CSV 파일이 성공적으로 읽어졌습니다.");
        }
        else
        {
            Debug.LogWarning("랭킹 CSV 파일이 존재하지 않습니다: " + rankingFilePath);
        }
    }

    public void WriteRankingCSV()
    {
        string rankingFilePath = Path.Combine(PathConfig.FolderPath, PathConfig.RankingFileName);

        using (StreamWriter writer = new StreamWriter(rankingFilePath))
        {
            writer.WriteLine("Time,Name,Trial");

            foreach (var record in _ranking)
            {
                string line = $"{record.Item1},{record.Item2},{record.Item3}";
                writer.WriteLine(line);
            }
        }

        Debug.Log("랭킹 CSV 파일이 성공적으로 작성되었습니다.");
    }

    public void UpdateRanking(float recordTime, string name, int trial)
    {
        // 현재 기록된 시간을 랭킹에 추가 (name, recordTime, trial)
        _ranking.Add(new Tuple<float, string, int>(recordTime, name, trial));

        // 시간을 기준으로 랭킹 정렬 (오름차순)
        _ranking.Sort((x, y) => x.Item1.CompareTo(y.Item1));

        // 중복 이름 처리: 중복된 이름이 있으면, 더 나은 기록만 남기고 나머지를 제거
        for (int i = _ranking.Count - 1; i > 0; i--)
        {
            if (_ranking[i].Item2 == _ranking[i - 1].Item2)
            {
                _ranking.RemoveAt(i);  // 뒤쪽의 기록을 삭제 (더 나은 기록이 앞에 있음)
            }
        }

        // 랭킹이 많아질 경우, 상위 n개만 유지 (예: 10개)
        int maxRankings = 10;
        if (_ranking.Count > maxRankings)
        {
            _ranking = _ranking.GetRange(0, maxRankings);  // 상위 n개의 기록만 유지
        }

        Debug.Log("랭킹이 업데이트되었습니다.");
    }
}
