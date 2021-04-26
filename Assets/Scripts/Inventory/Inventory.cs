using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory
{
    private GameObject[] slots;
    private int[] stock;


    public Inventory()
    {
        this.slots = GameHandler.Instance.slots;
        this.stock = GameHandler.Instance.stock;
    }

    public IEnumerator AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].name == item.name)
            {
                stock[i]++;
                slots[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = stock[i].ToString();
                break;
            }
        }
        yield return null;
    }

    public bool TryRemoveModule(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].name == item.name)
            {
                if (stock[i] <= 0)
                {
                    return false;
                }
                else
                {
                    stock[i]--;
                    slots[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = stock[i].ToString();
                    return true;
                }
            }
        }
        return false;
    }

    // For debugging purposes - add 100 of each item to the inventory
    public void CheatAddItems()
    {
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = 100;
            slots[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = stock[i].ToString();
        }
    }
}
