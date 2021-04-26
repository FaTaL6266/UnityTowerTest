using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameHandler : MonoBehaviour, IPointerClickHandler
{
    public static GameHandler Instance { get; private set; }
    public Inventory inventory;
    public GameObject[] slots;
    public int[] stock;

    // Text variables
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI towerCostText;
    [SerializeField] private TextMeshProUGUI moveCostText;
    [SerializeField] private Image healthBar;

    // Button variables
    [SerializeField] public Button buyButton;

    // Other references
    public InventoryMenu inventoryMenu;
    public bool bIsGameOver = false;
    public bool bDebugMode = false;
    private Animator animator;

    // Events
    public static event Action ApplyGameOver;

    #region Lives
    [SerializeField] private float lives;

    public void IncreaseLives(int value)
    {
        lives += value;
        UpdateUI();
    }

    public void DecreaseLives(int value)
    {
        if (!bIsGameOver && !bDebugMode)
        {
            lives -= value;
            UpdateUI();
            if (lives <= 0) GameOver();
        }
    }

    private void GameOver()
    {
        if (!bIsGameOver)
        {
            bIsGameOver = true;
            ApplyGameOver?.Invoke();
            if (bIsBuyingTower)
            {
                bIsBuyingTower = false;
                Destroy(followTower);
            }
            animator.SetTrigger("GameOver");
            buyButton.interactable = false;
            transform.Find("UI/GameOver").gameObject.SetActive(true);
        }
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
        UpdateUI();
    }

    public void DecreaseMoney(int value)
    {
        money -= value;
        UpdateUI();
    }

    public void IncreaseTowerCost()
    {
        towerCost = (int)(towerCost * 1.20);
        moveCost = (int)(towerCost / 3);
        UpdateUI();
    }

    public void DecreaseTowerCost()
    {
        towerCost = (int)(towerCost * 0.95);
        moveCost = (int)(towerCost / 3);
        UpdateUI();
    }
    #endregion

    #region UI
    public GameObject followTower;

    // Methods for the UI buttons on screen
    public void BuyButton()
    {
        if (!bIsBuyingTower)
        {
            bIsBuyingTower = true;
            inventoryMenu.HideMenu();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            followTower = Instantiate(GameAssets.Instance.dummyTower, mousePosition, Quaternion.identity, transform.Find("Background/PlayArea"));

            ToggleButtons();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CancelPurchase();
        }
    }

    public void CancelPurchase()
    {
        if (followTower)
        {
            Destroy(followTower);
            bIsBuyingTower = false;
            buyButton.interactable = true;
        }
    }

    public void ToggleButtons()
    {
        if (bIsBuyingTower || bIsMovingTower) buyButton.interactable = false;
        else buyButton.interactable = true;
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = (float)(lives / 100);
        livesText.text = lives.ToString();
        moneyText.text = "$" + money;
        towerCostText.text = "Buy Tower:\n$" + towerCost;
        moveCostText.text = "Move Cost:\n$" + moveCost;
    }
    #endregion

    private void Awake()
    {
        Instance = this;
        if (MusicManager.Instance)
        {
            Destroy(MusicManager.Instance.gameObject);
        }
        animator = transform.Find("Background").GetComponent<Animator>();
        GetComponent<PauseController>().UnpauseGame();
        inventory = new Inventory();
        UpdateUI();
        buyButton.interactable = true;
    }
}