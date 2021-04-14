using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Placement : MonoBehaviour, IPointerDownHandler, IDropHandler
{
    public bool bIsPlacementOccupied;

    // Start is called before the first frame update
    private void Awake()
    {
        bIsPlacementOccupied = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!bIsPlacementOccupied && eventData.button == PointerEventData.InputButton.Left)
        {
            if (GameHandler.Instance.bIsBuyingTower && GameHandler.Instance.TryBuyTower())
            {
                bIsPlacementOccupied = true;
                GameObject tower = Instantiate(GameAssets.Instance.tower, gameObject.transform.position, 
                    Quaternion.identity, GameObject.Find("GameHandler/Background/PlayArea/Towers").transform);
                tower.GetComponent<Tower>().ChangePlacement(this);

                if (this.gameObject.transform.localScale.x < 0)
                {
                    tower.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    tower.transform.localScale = new Vector3(1f, 1f, 0f);
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.gameObject.CompareTag("Tower") && !bIsPlacementOccupied)
        {
            bIsPlacementOccupied = true;
            GameHandler.Instance.TryMoveTower(true);
            eventData.pointerDrag.gameObject.GetComponent<Tower>().ChangePlacement(this);
            eventData.pointerDrag.gameObject.transform.position = gameObject.transform.position;
            if (this.gameObject.transform.localScale.x < 0)
            {
                eventData.pointerDrag.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                eventData.pointerDrag.gameObject.transform.localScale = new Vector3(1f, 1f, 0f);
            }
        }
    }
}
