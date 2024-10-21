using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TitleManager : MonoBehaviour
{
    public WheelchairUI wc;
    public NameSelector ns;

    public List<TitleElement> menu = new List<TitleElement>();
    public int currentMenuIdx = 0;
    
    void InitTitle()
    {
        if (menu == null || menu.Count == 0)
        {
            Debug.LogError("메뉴가 초기화되지 않았습니다.");
            return;
        }
        
        foreach (TitleElement menuItem in menu)
        {
            menuItem._onCursorOutOfBoundsLeft.AddListener(PrevMenu);            
            menuItem._onCursorOutOfBoundsRight.AddListener(NextMenu);            
        }

        AddHandler(0);
        menu[0].OnSelected();
    }

    void RemoveHandler(int menuIdx)
    {
        wc.onLeftForwardRotation.RemoveListener(menu[menuIdx].HandleLeftForwardRotation);
        wc.onLeftBackwardRotation.RemoveListener(menu[menuIdx].HandleLeftBackwardRotation);
        wc.onRightForwardRotation.RemoveListener(menu[menuIdx].HandleRightForwardRotation);
        wc.onRightBackwardRotation.RemoveListener(menu[menuIdx].HandleRightBackwardRotation);
    }
    void AddHandler(int menuIdx)
    {
        wc.onLeftForwardRotation.AddListener(menu[menuIdx].HandleLeftForwardRotation);
        wc.onLeftBackwardRotation.AddListener(menu[menuIdx].HandleLeftBackwardRotation);
        wc.onRightForwardRotation.AddListener(menu[menuIdx].HandleRightForwardRotation);
        wc.onRightBackwardRotation.AddListener(menu[menuIdx].HandleRightBackwardRotation);
    }
    
    void Start()
    {
        InitTitle();
    }

    public void PrevMenu()
    {
        if (currentMenuIdx > 0)
        {
            Debug.Log("PrevMenu");
            
            RemoveHandler(currentMenuIdx);
            menu[currentMenuIdx].OnUnselected();
            
            currentMenuIdx--;
            
            AddHandler(currentMenuIdx);
            menu[currentMenuIdx].OnSelected();
        }
        else
        {
            Debug.Log("Already First Menu");
        }
    }
    public void NextMenu()
    {
        if (currentMenuIdx < menu.Count - 1)
        {
            Debug.Log("NextMenu");
            
            RemoveHandler(currentMenuIdx);
            menu[currentMenuIdx].OnUnselected();

            currentMenuIdx++;
            
            AddHandler(currentMenuIdx);
            menu[currentMenuIdx].OnSelected();
        }
        else
        {
            Debug.Log("Already Last Menu");
        }
    }
}
