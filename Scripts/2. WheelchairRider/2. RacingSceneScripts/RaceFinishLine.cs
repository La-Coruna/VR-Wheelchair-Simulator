using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFinishLine : MonoBehaviour
{
    // 충돌 시 호출할 GameManager의 GoalIn 함수 참조
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // GameManager 찾기 (GameManager가 씬에 있는 경우)
        _gameManager = FindObjectOfType<GameManager>();
    }

    // 트리거 충돌 처리
    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때만 처리
        if (other.CompareTag("Player"))
        {
            // GameManager의 GoalIn 함수 호출
            if (_gameManager != null)
            {
                _gameManager.GoalIn();
                Debug.Log("충돌 감지");
            }
            else
            {
                Debug.LogWarning("GameManager가 설정되지 않았습니다!");
            }
        }
    }
}
