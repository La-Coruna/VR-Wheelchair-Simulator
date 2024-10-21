using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WheelchairUI : MonoBehaviour
{
    public bool isDebug = false;
    public float threshold = 40f;  // 이벤트가 발생하는 임계값

    // 왼쪽 및 오른쪽 바퀴의 시리얼 컨트롤러
    public SerialController leftSerialController;
    public SerialController rightSerialController;

    // 왼쪽 바퀴가 앞으로 굴렸을 때 발생하는 이벤트
    public UnityEvent onLeftForwardRotation;

    // 왼쪽 바퀴가 뒤로 굴렸을 때 발생하는 이벤트
    public UnityEvent onLeftBackwardRotation;

    // 오른쪽 바퀴가 앞으로 굴렸을 때 발생하는 이벤트
    public UnityEvent onRightForwardRotation;

    // 오른쪽 바퀴가 뒤로 굴렸을 때 발생하는 이벤트
    public UnityEvent onRightBackwardRotation;

    // 왼쪽 바퀴와 오른쪽 바퀴의 상태 기록
    private bool isLeftMoving = false;
    private bool isRightMoving = false;

    // 초기화 함수
    void Start()
    {
    }

    // 물리 업데이트 함수
    void FixedUpdate()
    {
        // 왼쪽 바퀴 시리얼 메시지 처리
        HandleWheelRotation(leftSerialController, ref isLeftMoving, onLeftForwardRotation, onLeftBackwardRotation);

        // 오른쪽 바퀴 시리얼 메시지 처리
        HandleWheelRotation(rightSerialController, ref isRightMoving, onRightBackwardRotation, onRightForwardRotation);
    }

    // 바퀴 회전을 처리하는 함수
    void HandleWheelRotation(SerialController serialController, ref bool isMoving, UnityEvent positiveEvent, UnityEvent negativeEvent)
    {
        // 시리얼 메시지 읽기
        string message = serialController.ReadSerialMessage();

        // 메시지가 유효한 경우 처리
        if (message != null && message[0] == '#')
        {
            float inputValue = DecryptMessage(message);

            // 임계값을 초과하는지 확인 (절대값이 threshold 이상)
            if (Mathf.Abs(inputValue) > threshold)
            {
                //Debug.Log(message);

                if (!isMoving)
                {
                    if (inputValue > 0)  // 바퀴가 앞으로 굴러가기 시작
                        positiveEvent.Invoke(); // 앞으로 굴림 이벤트 호출

                    else if (inputValue < 0)  // 바퀴가 뒤로 굴러가기 시작
                        negativeEvent.Invoke(); // 뒤로 굴림 이벤트 호출
                    
                    isMoving = true;
                }
            }
            else if (message == null || Mathf.Abs(inputValue) < threshold)  // 바퀴가 멈춤
            {
                isMoving = false;
            }
        }
    }

    // 시리얼 메시지 해독 함수
    float DecryptMessage(string message)
    {
        // 메시지에서 데이터를 분리
        string[] s = message.Substring(1).Split('/');
        float inputGyroX = float.Parse(s[0]);
        float inputFloat = 0f;

        // 아주 미세한 회전 걸러내기. (threshold 조건)
        if ((-1 * threshold) < inputGyroX && inputGyroX < threshold)
        {
            inputFloat = 0;
        }
        // 8비트인 센서 데이터 범위가 맞는지 확인 (-255~255).
        else if (-255f < inputGyroX && inputGyroX < 255f)
        {
            inputFloat = (inputGyroX * 6.56f / 100);
        }

        return inputFloat;  // 최종 해독된 값 반환
    }

    void Update()
    {
        DebugInput();
    }
    
    void DebugInput()
    {
        if (!isDebug) return;

        // 왼쪽 바퀴 처리
        if (Input.GetKeyUp(KeyCode.Q))
        {
            onLeftForwardRotation.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            onLeftBackwardRotation.Invoke();
        }

        // 오른쪽 바퀴 처리
        if (Input.GetKeyUp(KeyCode.W))
        {
            onRightForwardRotation.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            onRightBackwardRotation.Invoke();
        }
    }
}
