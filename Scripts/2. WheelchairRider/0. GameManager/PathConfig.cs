using System;
using System.IO;

public static class PathConfig
{
    // 폴더 경로
    public static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WheelchairRider");
    
    // 파일 이름
    public static readonly string RankingFileName = "_Ranking.csv";
    
    // 최대 랭킹 개수
    public static readonly int MaxRankings = 10;
}