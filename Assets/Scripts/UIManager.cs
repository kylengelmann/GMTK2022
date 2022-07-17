using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public CoinButton rerollButton;
    public CoinButton towerButton;
    public Text coinCount;

    public GameObject rerollPanel;
    public Text rerollText;

    public GameObject rollPanel;
    public void OnCoinsUpdated(int newCoinCount)
    {
        rerollButton.UpdateCoins(newCoinCount);
        towerButton.UpdateCoins(newCoinCount);

        coinCount.text = newCoinCount.ToString();
    }

    public void OnRerollClicked()
    {
        GameManager.gameManager.AddCoins(-rerollButton.cost);
        GameManager.gameManager.SetIsReroll(true);
    }

    public void NewTowerClicked()
    {
        GameManager.gameManager.AddCoins(-towerButton.cost);
        GameManager.gameManager.SetIsRolling(true, 1);
    }

    public void OnThrowingChanged(bool bNewThrowing)
    {
        rollPanel.SetActive(bNewThrowing);

        rerollButton.OnThrowingChanged(bNewThrowing);
        towerButton.OnThrowingChanged(bNewThrowing);
    }

    public void UpdateReroll(int numReroll)
    {
        if(numReroll <= 0)
        {
            rerollPanel.SetActive(false);
        }
        else
        {
            rerollPanel.SetActive(true);
            rerollText.text = "Select " + numReroll + " more towers to re-roll";
        }
    }
}