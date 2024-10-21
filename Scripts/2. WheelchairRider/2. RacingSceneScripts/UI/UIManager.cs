using System.Collections;
using UnityEngine;
using TMPro; 
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public CountdownManager _countdownManager;
    public TextMeshProUGUI _lapText;
    public TextMeshProUGUI _coinText;
    public TextMeshProUGUI _centerText;

    public void UpdateLapCount(int remainingLaps)
    {
        _lapText.text = $"Lap: {remainingLaps}/{GameConfig.LAP_MAX}"; 
    }
    public void UpdateCoinCount(int currentCoin)
    {
        _coinText.text = $"Coin: {currentCoin}/{GameConfig.COIN_MAX}";
    }

    public void DisplayWinMessage()
    {
        _centerText.gameObject.SetActive(true);
        _centerText.text = "WIN!!!";
        _centerText.color = Color.red;
    }

    public void DisplayLoseMessage()
    {
        _centerText.gameObject.SetActive(true);
        _centerText.text = "Lose...";
        _centerText.color = Color.blue;
    }
}
