using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    bool bIsRolling;

    public static GameManager gameManager;

    public int Coins{get; private set; }

    public UIManager uiManager;

    public DieThrower thrower;

    public int NumReroll= 3;
    public int InitialRoll = 4;

    List<GameObject> rerollTowers = new List<GameObject>();
    GameObject towerHovering;

    bool bReroll;

    private void Awake()
    {
        gameManager = this;
        UpdateCoins();
        uiManager.UpdateReroll(0);
    }

    private void Start()
    {
        SetIsRolling(true, InitialRoll);
    }

    public void AddCoins(int coins)
    {
        Coins = Mathf.Max(0, Coins + coins);
        UpdateCoins();
    }

    void UpdateCoins()
    {
        uiManager.OnCoinsUpdated(Coins);
    }

    public void SetIsRolling(bool bNewIsRolling, int numRolling = 0)
    {
        if(bNewIsRolling != bIsRolling)
        {
            bIsRolling = bNewIsRolling;

            thrower.enabled = bNewIsRolling;
            thrower.diceToThrow = numRolling;

            uiManager.OnThrowingChanged(bNewIsRolling);
        }
    }

    public void SetIsReroll(bool bIsReroll)
    {
        if(bIsReroll != bReroll)
        {
            bReroll = bIsReroll;
            if(!bReroll)
            {
                rerollTowers.Clear();
                towerHovering = null;
            }

            uiManager.UpdateReroll(bReroll ? NumReroll : 0);
        }
    }

    private void Update()
    {
        if(!bReroll) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetIsReroll(false);
            Coins += uiManager.rerollButton.cost;
            UpdateCoins();
            return;
        }

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        GameObject newHover;
        if(Physics.Raycast(mouseRay, out hit, 20f, 1 << LayerMask.NameToLayer("TowerUI")))
        {
            newHover = hit.collider.transform.parent.gameObject;
            if(rerollTowers.Contains(newHover))
            {
                newHover = null;
            }
        }
        else
        {
            newHover = null;
        }

        if(newHover != towerHovering)
        {
            if(towerHovering)
            {
                towerHovering.GetComponent<DamageTower>().bIsHovered = false;
            }
            
            if(newHover)
            {
                newHover.GetComponent<DamageTower>().bIsHovered = true;
            }
        }

        towerHovering = newHover;

        if(towerHovering && Input.GetMouseButtonDown(0))
        {
            rerollTowers.Add(towerHovering);
            towerHovering.GetComponent<DamageTower>().bIsSelected = true;

            if(rerollTowers.Count >= NumReroll)
            {
                foreach(GameObject go in rerollTowers)
                {
                    Destroy(go);
                }

                SetIsReroll(false);
                SetIsRolling(true, NumReroll);
            }
            else
            {
                uiManager.UpdateReroll(NumReroll - rerollTowers.Count);
            }
        }
    }
}
