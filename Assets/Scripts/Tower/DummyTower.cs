using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTower : MonoBehaviour
{
    // Private variables only changeable through script
    private Vector3 mousePosition;
    private float moveSpeed = 1.0f;
    private Tower tower;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }
}
