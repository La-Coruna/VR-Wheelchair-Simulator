using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardUI : MonoBehaviour
{
    // 왼쪽 바퀴가 앞으로 굴렸을 때 발생하는 이벤트
    public UnityEvent onLeftForwardRotation;

    // 왼쪽 바퀴가 뒤로 굴렸을 때 발생하는 이벤트
    public UnityEvent onLeftBackwardRotation;

    // 오른쪽 바퀴가 앞으로 굴렸을 때 발생하는 이벤트
    public UnityEvent onRightForwardRotation;

    // 오른쪽 바퀴가 뒤로 굴렸을 때 발생하는 이벤트
    public UnityEvent onRightBackwardRotation;


    // 초기화 함수
    void Start()
    {
    }

    // 물리 업데이트 함수
    void Update()
    {
        // 왼쪽 바퀴 처리
        if(Input.GetKeyUp(KeyCode.Q)) 
        { 
            onLeftForwardRotation.Invoke();
        } else if(Input.GetKeyUp(KeyCode.A))
        {
            onLeftBackwardRotation.Invoke();
        }

        // 오른쪽 바퀴 처리
        if (Input.GetKeyUp(KeyCode.Q)) 
        {
            onRightForwardRotation.Invoke();
        } else if(Input.GetKeyUp(KeyCode.A))
        {
            onRightBackwardRotation.Invoke();
        }
    }
}
