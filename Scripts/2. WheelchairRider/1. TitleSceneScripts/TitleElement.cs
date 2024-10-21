using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class TitleElement : MonoBehaviour, IWheelchairUI
{
    public TextMeshProUGUI _menuText;
    public UnityEvent _onCursorOutOfBoundsLeft;
    public UnityEvent _onCursorOutOfBoundsRight;

    // 꼭 OOB 상황을 처리해줘서 다음 메뉴로 넘어가든 하게 해야함.
    public void HandleCursorLeftOutOfBounds()
    {
        _onCursorOutOfBoundsLeft.Invoke();
    }

    public void HandleCursorRightOutOfBounds()
    {
        _onCursorOutOfBoundsRight.Invoke();
    }

    public abstract void HandleLeftForwardRotation();
    public abstract void HandleLeftBackwardRotation();
    public abstract void HandleRightForwardRotation();
    public abstract void HandleRightBackwardRotation();

    public virtual void OnSelected()
    {
        _menuText.fontStyle |= FontStyles.Underline;
    }
    public virtual void OnUnselected()
    {
        _menuText.fontStyle &= ~FontStyles.Underline;
    }
}
