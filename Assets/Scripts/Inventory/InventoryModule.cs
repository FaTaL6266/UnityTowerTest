using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryModule : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject spawnedAsset;
    private Inventory inventory;

    private void Awake()
    {
        {
            this.inventory = GameHandler.Instance.inventory;
            canvas = GameObject.Find("GameHandler/UI/UICanvas").GetComponent<Canvas>();

            rectTransform = GetComponent<RectTransform>();
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject module = GameAssets.Instance.GetModuleAsset(eventData.pointerDrag.gameObject.name);
        
        bool removed = inventory.TryRemoveModule(module);
        if (removed)
        {
            GameObject spawnedAsset = Instantiate(module, gameObject.transform.position, Quaternion.identity, canvas.transform);
            spawnedAsset.name = module.name;
            spawnedAsset.tag = "SpawnedFromInventory";
            spawnedAsset.GetComponent<DragDrop>().MoveFromInventory();
            eventData.pointerDrag = spawnedAsset;
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
    }
}
