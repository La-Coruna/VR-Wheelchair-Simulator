using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;


public static class PathDrawer
{
    public static float scaleFactor = 17.07f; // 이미지 크기 조절  (background image: 1024x512, byte path image: 60x30. => 1024/60 = 17.0666)
    public static float minX = -30f; // 이동 범위 최소 x 좌표 (오프셋 x)
    public static float minZ = -15f; // 이동 범위 최소 z 좌표 (오프셋 y)
    
    public static Texture2D backgroundImage; // 배경 이미지 (지도 이미지 넣을 것임.)

    static PathDrawer()
    {
        backgroundImage = Resources.Load<Texture2D>("BackgroundImage");
    }

    // PathRecorder를 v2로 바꾸면서 path를 저장하는 방식이 바뀜.
    public static void createImageWithPathRecord(List<Tuple<float, float, float, float, float, float, float>> pathRecord, List<Vector3> collisionPositions, string destPath)
    {
        List<Vector3> pathPoints = new List<Vector3>();

        foreach (var record in pathRecord)
        {
            pathPoints.Add(new Vector3(record.Item2, record.Item3, record.Item4));
        }

        createImageWithPathPoints(pathPoints, collisionPositions, destPath);
    }


    public static void createImageWithPathCSV(string csvPath, List<Vector3> collisionPositions, string destPath)
    {
        List<Vector3> pathPoints = ReadCSV(csvPath);
        createImageWithPathPoints(pathPoints , collisionPositions, destPath);
    }
    
    private static List<Vector3> ReadCSV(string csvPath)
    {
        List<Vector3> pathPoints = new List<Vector3>();;
        
        //읽어올 csv 파일을 resources 폴더로 복사함.
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Resources/PathRecord.csv");
        FileUtil.CopyFileOrDirectory(csvPath, Application.dataPath + "/Resources/PathRecord.csv");
        
        while(!File.Exists(Application.dataPath + "/Resources/PathRecord.csv"))
        {
        }
        
        // 복사한 파일을 읽음.
        List<Dictionary<string, object>> csvData = CSVReader.Read("PathRecord");

        for (int i = 0; i < csvData.Count; i++)
        {
            pathPoints.Add( new Vector3(float.Parse(csvData[i]["WCPos_x"].ToString()), 0, float.Parse(csvData[i]["WCPos_z"].ToString())) );
        }

        return pathPoints;
    }
    
    //public static void createImageWithPathPoints(List<Vector3> pathPoints, List<Vector3> collisionPositions, string destPath)
    public static void createImageWithPathPoints(List<Vector3> pathPoints, List<Vector3> collisionPositions, string destPath)
    {
        // 이미지 생성용 텍스쳐 생성
        Texture2D texture = new Texture2D(1024, 512);
        Color[] colors = new Color[texture.width * texture.height];
        for (int i = 0; i < colors.Length; i++)
        {
            // 배경 이미지로 색상 초기화
            int x = i % texture.width;
            int y = i / texture.width;
            colors[i] = backgroundImage.GetPixel(x, y);
        }
        texture.SetPixels(colors);

        // 이동 경로 그리기
        for (int i = 1; i < pathPoints.Count; i++)
        {
            Vector3 start = pathPoints[i - 1];
            Vector3 end = pathPoints[i];
            DrawLine(texture, start, end, Color.red);
        }
        
        // 충돌 지점 표시
        foreach (Vector3 collisionPos in collisionPositions)
        {
            int x = (int)((collisionPos.x - minX) * scaleFactor);
            int y = (int)((collisionPos.z - minZ) * scaleFactor);

            DrawCircle(texture, x, y, Color.yellow);
        }

        // 이미지 파일로 저장
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(destPath, bytes);

        pathPoints = null;
    }

    private static void DrawLine(Texture2D tex, Vector3 start, Vector3 end, Color color)
    {
        int x0 = (int)((start.x - minX) * scaleFactor);
        int y0 = (int)((start.z - minZ) * scaleFactor);
        int x1 = (int)((end.x - minX) * scaleFactor);
        int y1 = (int)((end.z - minZ) * scaleFactor);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            tex.SetPixel(x0, y0, color);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    
    static void DrawCircle(Texture2D tex, int cx, int cy, Color color)
    {
        int radius = 5; // 원의 반지름
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            tex.SetPixel(cx + x, cy + y, color);
            tex.SetPixel(cx + y, cy + x, color);
            tex.SetPixel(cx - y, cy + x, color);
            tex.SetPixel(cx - x, cy + y, color);
            tex.SetPixel(cx - x, cy - y, color);
            tex.SetPixel(cx - y, cy - x, color);
            tex.SetPixel(cx + y, cy - x, color);
            tex.SetPixel(cx + x, cy - y, color);

            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
    }

}
