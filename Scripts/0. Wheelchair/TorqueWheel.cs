using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전달받은 회전속도를 바탕으로 휠체어 바퀴를 굴러가게 하는 컴포넌트.
public class TorqueWheel : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float magnitude = 6.56f;  // 회전 속도를 조정하는 값
    public float threshold = 2f;  // 회전 속도를 멈추는 임계값
    // SerialController는 에셋을 사용. Ardity: Arduino + Unity communication made easy
    public SerialController serialController;  // 시리얼 통신 컨트롤러
    public static bool check_isUphill = false;  // 경사로 상태 확인 변수

    // 바퀴가 왼쪽인지 오른쪽인지 구분하는 변수 (true = 왼쪽, false = 오른쪽)
    public bool isLeftWheel;

    // 초기화 함수
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 가져오기
    }

    // 물리 업데이트 함수
    void FixedUpdate()
    {
        // 시리얼 메시지 읽기
        string message = serialController.ReadSerialMessage();

        // 메시지가 유효한 경우 처리
        if (message != null && message[0] == '#')
        {
            // 메시지를 해독하여 바퀴에 각속도 적용
            _rigidBody.angularVelocity = gameObject.transform.right * DecryptMessage(message);
        }
    }

    // 시리얼 메시지 해독 함수
    float DecryptMessage(string message)
    {
        // 메시지에서 데이터를 분리
        string[] s = message.Substring(1).Split('/');
        float inputGyroX = float.Parse(s[0]);
        float inputFloat = 0;

        // 아주 미세한 회전 걸러내기. (threshold 조건)
        if ((-1 * threshold) < inputGyroX && inputGyroX < threshold)
        {
            inputFloat = 0;
        }
        // 8비트인 센서 데이터 범위가 맞는지 확인 (-255~255).
        else if (-255f < inputGyroX && inputGyroX < 255f)
        {
            inputFloat = (inputGyroX * magnitude/100 );
        }

        // 경사로에서의 움직임 조정
        // 왼쪽 바퀴는 양수 값을, 오른쪽 바퀴는 음수 값을 경사로에서 감소시킴
        if (check_isUphill)
        {
            if (isLeftWheel && inputFloat < 0)  // 왼쪽 바퀴의 경우
            {
                inputFloat /= 3;
            }
            else if (!isLeftWheel && inputFloat > 0)  // 오른쪽 바퀴의 경우
            {
                inputFloat /= 3;
            }
        }

        return inputFloat;  // 최종 회전값 반환
    }
}
