using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingViewer : MonoBehaviour
{
    public GameObject rankingItemPrefab; // 각 순위 항목을 표시할 프리팹
    public Transform rankingContainer;   // 순위 항목들이 배치될 부모 오브젝트
    public float itemSpacing = 50f;      // 순위 항목 간 간격

    private List<GameObject> rankingItems = new List<GameObject>(); // 생성된 순위 항목들을 저장

    void Start()
    {
        ShowRanking();
    }

    // 순위 목록을 화면에 표시하는 함수
    void ShowRanking()
    {
        ClearRankingItems(); // 기존 항목 제거

        var ranking = Ranking.Instance.RankingList; // Ranking 클래스에서 순위 데이터 가져오기

        for (int i = 0; i < ranking.Count; i++)
        {
            // 프리팹을 생성하고 부모 컨테이너에 배치
            GameObject rankingItem = Instantiate(rankingItemPrefab, rankingContainer);

            // 항목의 위치를 설정 (아이템 간 간격을 고려)
            rankingItem.transform.localPosition = new Vector3(0, -i * itemSpacing, 0);

            // TextMeshPro 컴포넌트를 찾아서 텍스트 설정
            TMP_Text text = rankingItem.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = $"[{i + 1}]  {ranking[i].Item2}  {ranking[i].Item1:F2}";
            }

            rankingItems.Add(rankingItem); // 생성된 항목 저장
        }
    }

    // 기존에 생성된 순위 항목들을 제거하는 함수
    void ClearRankingItems()
    {
        foreach (var item in rankingItems)
        {
            Destroy(item); // 항목 파괴
        }
        rankingItems.Clear(); // 리스트 초기화
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void ReturnToTitle()
    {
        GameManager.Instance.LoadTitleScene();
    }
}
