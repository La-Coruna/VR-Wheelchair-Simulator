using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.Events;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public UnityEvent onCountdownStart; // 카운트다운이 끝났을 때 발생할 이벤트
    public UnityEvent onCountdownEnd; // 카운트다운이 끝났을 때 발생할 이벤트

    private void Start()
    {
        onCountdownStart.Invoke();
        StartCoroutine(CountdownRoutine()); // 게임 시작 시 자동으로 카운트다운 시작
    }

    private IEnumerator CountdownRoutine()
    {
        // 3, 2, 1 카운트다운 표시
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); // TMP 텍스트를 숫자로 변경

            // 숫자가 줄어들면서 색상을 빨간색으로 점진적으로 변경
            float colorFactor = (3f - i) / 3f; // 0일 때 흰색, 1일 때 빨간색
            countdownText.color = Color.Lerp(Color.white, Color.red, colorFactor); // 흰색에서 빨간색으로 변환

            yield return new WaitForSeconds(1f); // 1초 대기
        }

        // 카운트다운이 끝나면 "시작" 메시지 표시
        countdownText.text = "Race Start!";
        countdownText.color = Color.red; // "시작" 텍스트는 빨간색으로 설정
        yield return new WaitForSeconds(1f); // 1초 대기

        countdownText.gameObject.SetActive(false); // 텍스트 UI 숨기기

        // 카운트다운이 끝났음을 알리는 Unity 이벤트 발생
        onCountdownEnd.Invoke();
    }
}
