using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSelector : TitleElement
{
    public TextMeshProUGUI[] _text = new TextMeshProUGUI[3];
    public char[] _c = new char[3];
    public int _cursor = 0;
    public string Name
    {
        get { return new string(_c); }
    }

    void Start()
    {
        _c[0] = 'A';
        _c[1] = 'A';
        _c[2] = 'A';
        
        _onCursorOutOfBoundsRight.AddListener(SelectName);
    }

    void Update()
    {
        _text[0].text = $"{_c[0]}";
        _text[1].text = $"{_c[1]}";
        _text[2].text = $"{_c[2]}";
    }

    public void NextAlpha()
    {
        if (_c[_cursor] < 'Z') _c[_cursor]++;
        else _c[_cursor] = 'A';
    }
    public void PrevAlpha()
    {
        if (_c[_cursor] > 'A') _c[_cursor]--;
        else _c[_cursor] = 'Z';
    }

    public void NextCursor()
    {
        _text[_cursor].fontStyle &= ~FontStyles.Underline;
        if (_cursor < 2)
        {
            _cursor++;
            _text[_cursor].fontStyle |= FontStyles.Underline;
        }
        else HandleCursorRightOutOfBounds();
    }
    public void PrevCursor()
    {
        _text[_cursor].fontStyle &= ~FontStyles.Underline;
        if (_cursor > 0)
        {
            _cursor--;
            _text[_cursor].fontStyle |= FontStyles.Underline;
        }
        else HandleCursorLeftOutOfBounds();
    }

    public override void HandleLeftForwardRotation()
    {
        NextCursor();
    }

    public override void HandleLeftBackwardRotation()
    {
        PrevCursor();
    }

    public override void HandleRightForwardRotation()
    {
        NextAlpha();
    }

    public override void HandleRightBackwardRotation()
    {
        PrevAlpha();
    }
    
    public override void OnSelected()
    {
        base.OnSelected();
        _text[_cursor].fontStyle |= FontStyles.Underline;
    }
    public override void OnUnselected()
    {
        base.OnUnselected();
        _text[_cursor].fontStyle &= ~FontStyles.Underline;
    }

    public void SelectName()
    {
        Debug.Log("이름 설정 완료");
        PlayerPrefs.SetString("Name", this.Name);
    }
}
