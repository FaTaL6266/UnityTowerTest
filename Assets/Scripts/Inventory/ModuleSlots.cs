using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleSlots : MonoBehaviour, IDropHandler
{
    private Inventory inventory;
    private InventoryMenu inventoryMenu;

    private void Awake()
    {
        this.inventory = GameHandler.Instance.inventory;
        inventoryMenu = GameObject.Find("GameHandler/UI/UICanvas/TowerInfoPanel").GetComponent<InventoryMenu>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && inventoryMenu.bIsShowingTowerData && inventoryMenu.tower)
        {
            string nameOfAsset = eventData.pointerDrag.gameObject.GetComponent<Image>().sprite.name + "_Box";
            Sprite moduleAsset = GameAssets.Instance.GetAppliedModuleAsset(nameOfAsset);
            GetComponent<Image>().sprite = moduleAsset;
            StartCoroutine(ApplyModifiers(eventData.pointerDrag.gameObject));
            Destroy(eventData.pointerDrag.gameObject);
        }
        else if (eventData.pointerDrag != null && eventData.pointerDrag.gameObject.CompareTag("SpawnedFromInventory"))
        {
            eventData.pointerDrag.gameObject.GetComponent<DragDrop>().ReturnToInventory();
        }
        else if (eventData.pointerDrag != null && eventData.pointerDrag.gameObject.CompareTag("SpawnedOnEnemyDeath"))
        {
            eventData.pointerDrag.gameObject.GetComponent<DragDrop>().ResetPosition();
        }
    }

    private IEnumerator ApplyModifiers(GameObject module)
    {
        string slotName = gameObject.name;

        switch (slotName)
        {
            case "Module_1": StartCoroutine(inventoryMenu.tower.ApplyModule(module, 0)); break;
            case "Module_2": StartCoroutine(inventoryMenu.tower.ApplyModule(module, 1)); break;
            case "Module_3": StartCoroutine(inventoryMenu.tower.ApplyModule(module, 2)); break;
            case "Module_4": StartCoroutine(inventoryMenu.tower.ApplyModule(module, 3)); break;
        }
        inventoryMenu.ShowMenuWithTowerStats(inventoryMenu.tower);
        yield return null;
    }

    public void NewTowerSelected(GameObject module)
    {
        string nameOfAsset = module.GetComponent<Image>().sprite.name + "_Box";
        Sprite moduleAsset = GameAssets.Instance.GetAppliedModuleAsset(nameOfAsset);
        GetComponent<Image>().sprite = moduleAsset;
    }
}
