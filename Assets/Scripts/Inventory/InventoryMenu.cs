using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : MonoBehaviour, IDropHandler
{
    private Inventory inventory;

    private TextMeshProUGUI healthText;
    private TextMeshProUGUI physicalDamageText;
    private TextMeshProUGUI fireDamageText;
    private TextMeshProUGUI fireRateText;
    private TextMeshProUGUI physicalResistanceText;
    private TextMeshProUGUI fireResistanceText;

    public Tower tower;

    public bool bIsVisible;
    public bool bIsShowingTowerData;

    private void Awake()
    {
        GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>().OnTowerSelected += ShowMenuWithTowerStats;
        GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>().OnModuleDrag += ShowMenuWithoutTowerStats;

        bIsVisible = true;
        bIsShowingTowerData = false;

        GameHandler.ApplyGameOver += GameOver;

        healthText =             transform.Find("TowerInfo/TowerDetailsText/HealthText").GetComponent<TextMeshProUGUI>();
        physicalDamageText =     transform.Find("TowerInfo/TowerDetailsText/PhysicalDamageText").GetComponent<TextMeshProUGUI>();
        fireDamageText =         transform.Find("TowerInfo/TowerDetailsText/FireDamageText").GetComponent<TextMeshProUGUI>();
        fireRateText =           transform.Find("TowerInfo/TowerDetailsText/FireRateText").GetComponent<TextMeshProUGUI>();
        physicalResistanceText = transform.Find("TowerInfo/TowerDetailsText/PhysicalResistanceText").GetComponent<TextMeshProUGUI>();
        fireResistanceText =     transform.Find("TowerInfo/TowerDetailsText/FireResistanceText").GetComponent<TextMeshProUGUI>();
    }

    private void GameOver()
    {
        HideMenu();
    }

    private void Start()
    {
        HideMenu();
        this.inventory = GameHandler.Instance.inventory;
    }

    public void ShowMenuWithTowerStats(Tower tower)
    {
        this.tower = tower;
        bIsShowingTowerData = true;
        bIsVisible = true;
        healthText.text = "Health: " + tower.Health.ToString();
        physicalDamageText.text = "Physical Damage: " + tower.PhysicalDamage.ToString();
        fireDamageText.text = "Fire Damage: " + tower.FireDamage.ToString();
        fireRateText.text = "Fire Rate: " + tower.FireRate.ToString();
        physicalResistanceText.text = "Physical Resistance: " + tower.PhysicalResistance.ToString();
        fireResistanceText.text = "Fire Resistance: " + tower.FireResistance.ToString();
        gameObject.SetActive(true);
    }

    private void ShowMenuWithoutTowerStats()
    {
        if (!bIsVisible)
        {
            bIsShowingTowerData = false;
            bIsVisible = true;
            healthText.text = "No tower selected";
            physicalDamageText.text = "No tower selected";
            fireDamageText.text = "No tower selected";
            fireRateText.text = "No tower selected";
            physicalResistanceText.text = "No tower selected";
            fireResistanceText.text = "No tower selected";
            gameObject.SetActive(true);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && !eventData.pointerDrag.gameObject.CompareTag("Tower"))
        {
            StartCoroutine(inventory.AddItem(eventData.pointerDrag.gameObject));
            Destroy(eventData.pointerDrag.gameObject);
        }
    }

    public void HideMenu()
    {
        this.tower = null;
        if (bIsVisible)
        {
            bIsShowingTowerData = false;
            bIsVisible = false;
            gameObject.SetActive(false);
        }
    }
}
