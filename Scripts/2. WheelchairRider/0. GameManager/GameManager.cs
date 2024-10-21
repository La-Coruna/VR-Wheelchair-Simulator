using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string _name;
    
    public PathRecorder_v2 _pathRacorder;
    public ReplayManager_v2 _replayManager;
    public WheelchairManager _wheelchair;

    bool _isRaceStart = false;

    public int _coinCount = 0;

    public UIManager _ui;

    public GameObject _coins;

    public bool _isWin = false;


    #region Variable for Race
    bool isFinish = false; // 완주 했는지
    int _lap = 0;

#endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 존재하면 중복 인스턴스 제거
        }
    }

    public void RaceInit()
    {
        isFinish = false; // 완주 했는지
        _lap = 0;
        _replayManager.ReadCSV_BestRecord();
    }

    // 게임 시작 함수
    public void RaceStart()
    {
        // 관련 변수들 초기화

        // 플레이어 움직일 수 있도록하기
        AllowMove();

        // 기록 시작하기
        _pathRacorder.StartRecord();

        // 리플레이모드 시작하기
        _replayManager.StartReplay();

        _isRaceStart = true;
    }

    public bool CheckCoin()
    {
        return _coinCount == GameConfig.COIN_MAX;
    }

    public void ShowNextCoin()
    {
        _coinCount = 0;
        _ui.UpdateCoinCount(_coinCount);
        // 현재 Lap 코인 비활성화
        Transform currentLapCoins = _coins.transform.Find(_lap + "Lap");
        if (currentLapCoins != null)
        {
            currentLapCoins.gameObject.SetActive(false);
        }

        // 다음 Lap 코인 활성화
        Transform nextLapCoins = _coins.transform.Find((_lap + 1) + "Lap");
        if (nextLapCoins != null)
        {
            nextLapCoins.gameObject.SetActive(true);
        }
    }

    public void GoalIn()
    {
        if (CheckCoin())
        {
            IncreaseLap();
            ShowNextCoin();
        }
    }
    public void IncreaseLap()
    {
        _lap++;
        _ui.UpdateLapCount(_lap);

        // 게임 종료
        if (_lap == GameConfig.LAP_MAX)
        {
            isFinish = true;
            RaceEnd();
            if (_isWin)
            {
                _ui.DisplayLoseMessage();
            }
            else
            {
                _ui.DisplayWinMessage();
            }
        }
    }

    public void RaceEnd()
    {
        // 플레이어 못 움직이도록.
        ProhibitMove();

        // 기록 종료.
        _pathRacorder.EndRecord();

        // 리플레이모드 종료.
        _replayManager.StopReplay();

        // 이겼는지 확인
        CheckIfWin();

        _isRaceStart = false;

        StartCoroutine(LoadRankingSceneAfterDelay(2f));
    }

    private IEnumerator LoadRankingSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Ranking 씬으로 이동
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ranking");
    }

    // 플레이어의 움직임을 허락하는 함수.
    public void AllowMove()
    {
        _wheelchair.AllowMove();
    }

    // 플레이어의 움직임을 제한하는 함수.
    public void ProhibitMove()
    {
        _wheelchair.ProhibitMove();
    }

    public void CheckIfWin()
    {
        _isWin = _replayManager.IsReplayDone();
    }

    // 코인 개수를 증가시키는 함수
    public void AddCoin()
    {
        _coinCount++;
        _ui.UpdateCoinCount(_coinCount);
        Debug.Log("코인을 먹었습니다! 현재 코인 개수: " + _coinCount);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void LoadTitleScene()
    {
        Debug.Log("Title Scene으로 이동합니다.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
