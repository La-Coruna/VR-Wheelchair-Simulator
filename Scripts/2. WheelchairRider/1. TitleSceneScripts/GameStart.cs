using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : TitleElement
{
    public void StartGame()
    {
        SceneManager.LoadScene("WheelchairRider/Scenes/Game");
    }
    
    public override void HandleLeftForwardRotation()
    {
        HandleCursorRightOutOfBounds();
    }

    public override void HandleLeftBackwardRotation()
    {
        HandleCursorLeftOutOfBounds();
    }

    public override void HandleRightForwardRotation()
    {
        StartGame();
    }

    public override void HandleRightBackwardRotation()
    {
        StartGame();
    }
}
