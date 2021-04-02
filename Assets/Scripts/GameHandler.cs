using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }
    public Inventory inventory;
    public GameObject[] slots;
    public int[] stock;

    // Text variables
    [SerializeField] private Text livesText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text towerCostText;

    // Button variables
    [SerializeField] public Button cancelButton;
    [SerializeField] public Button buyButton;

    // Other references
    public InventoryMenu inventoryMenu;


    #region Lives
    private int lives = 100;

    public void IncreaseLives(int value)
    {
        lives += value;
    }

    public void DecreaseLives(int value)
    {
        lives -= value;
        if (lives <= 0) GameOver();
    }

    private void GameOver()
    {
        Debug.Log("The game is over");
    }
    #endregion

    #region Money
    private int money = 500;
    private int towerCost = 100;
    private int moveCost = 33;
    public bool bIsBuyingTower = false;
    public bool bIsMovingTower = false;

    public bool TryBuyTower()
    {
        if (money >= towerCost)
        {
            DecreaseMoney(towerCost);
            IncreaseTowerCost();
            return true;
        }
        else return false;
    }

    public bool TryMoveTower(bool moving)
    {
        if (money >= moveCost)
        {
            if (moving)
            {
                DecreaseMoney(moveCost);
            }
            return true;
        }
        else return false;
    }

    public void IncreaseMoney(int value)
    {
        money += value;
        UpdateText();
    }

    public void DecreaseMoney(int value)
    {
        money -= value;
        UpdateText();
    }

    public void IncreaseTowerCost()
    {
        towerCost = (int)(towerCost * 1.33);
        moveCost = (int)(towerCost / 3);
        UpdateText();
    }

    public void DecreaseTowerCost()
    {
        towerCost = (int)(towerCost * 0.95);
        moveCost = (int)(towerCost / 3);
        UpdateText();
    }
    #endregion

    #region UI
    private GameObject followTower;
    // Methods for the UI buttons on screen
    public void BuyButton()
    {
        if (!bIsBuyingTower)
        {
            bIsBuyingTower = true;
            inventoryMenu.HideMenu();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            followTower = Instantiate(GameAssets.Instance.dummyTower, mousePosition, Quaternion.identity, transform.Find("Background/PlayArea"));

            ToggleButtons();
        }
    }

    public void CancelButton()
    {
        if (bIsBuyingTower)
        {
            bIsBuyingTower = false;
            Destroy(followTower);

            ToggleButtons();
        }
    }

    public void ToggleButtons()
    {
        if (bIsBuyingTower || bIsMovingTower) { cancelButton.interactable = true; buyButton.interactable = false; }
        else { cancelButton.interactable = false; buyButton.interactable = true; }
    }

    private void UpdateText()
    {
        livesText.text = "Lives: " + lives;
        moneyText.text = "Money: " + money;
        towerCostText.text = "Buy Tower: " + towerCost;
    }
    #endregion

    private void Awake()
    {
        Instance = this;
        inventory = new Inventory();
        UpdateText();
        cancelButton.interactable = false;
        buyButton.interactable = true;
    }
}