using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Inventory inventory;
    private UIBehaviourManager manager;

    public ModuleRarity moduleRarity;
    public ModuleType moduleType;

    private void Awake()
    {
        this.inventory = GameHandler.Instance.inventory;
        canvas = GameObject.Find("GameHandler/Background").GetComponent<Canvas>();
        manager = GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (gameObject.CompareTag("SpawnedOnEnemyDeath"))
        {
            Invoke("AutomaticAddToInventory", 10f);
        }
    }

    private void AutomaticAddToInventory()
    {
        StartCoroutine(inventory.AddItem(gameObject));
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameHandler.Instance.inventoryMenu.bIsVisible)
        {
            manager.ModuleDragging();
        }
        CancelInvoke("AutomaticAddToInventory");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.CompareTag("SpawnedFromInventory"))
        {
            ReturnToInventory();
        }
        else if(gameObject.CompareTag("SpawnedOnEnemyDeath"))
        {
            ResetPosition();
            Invoke("AutomaticAddToInventory", 10f);
        }
    }

    public void ReturnToInventory()
    {
        StartCoroutine(inventory.AddItem(gameObject));
        Destroy(gameObject);
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void MoveFromInventory()
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }
}
