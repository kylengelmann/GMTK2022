using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinButton : MonoBehaviour
{
    public Button button;
    public Text costText;
    public int cost;

    bool bDisabled;

    private void Awake()
    {
        costText.text = cost.ToString();
    }

    public void UpdateCoins(int newCoins)
    {
        button.interactable = newCoins >= cost && !bDisabled;
    }

    public void OnThrowingChanged(bool bNewThrowing)
    {
        bDisabled = bNewThrowing;
        button.interactable = GameManager.gameManager.Coins >= cost && !bDisabled;
    }
}
