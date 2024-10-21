using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWheelchairUI
{
    // 왼쪽 바퀴가 앞으로 굴렸을 때 호출될 함수
    public abstract void HandleLeftForwardRotation();
    public abstract void HandleLeftBackwardRotation();
    public abstract void HandleRightForwardRotation();
    public abstract void HandleRightBackwardRotation();
}
