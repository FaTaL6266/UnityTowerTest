using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppliedModules : MonoBehaviour
{
    private Sprite defaultSlotSprite;

    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private GameObject slot4;

    private void Awake()
    {
        GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>().OnTowerSelected += ShowMenuWithTowerModules;
        GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>().OnModuleDrag += ShowMenuWithDefaultModuleSlots;

        defaultSlotSprite = GameAssets.Instance.defaultSlotSprite;

        slot1 = GameObject.Find("Module_1");
        slot2 = GameObject.Find("Module_2");
        slot3 = GameObject.Find("Module_3");
        slot4 = GameObject.Find("Module_4");
    }

    private void ShowMenuWithTowerModules(Tower tower)
    {
        if (!String.IsNullOrEmpty(tower.modules[0])) slot1.GetComponent<ModuleSlots>().NewTowerSelected(GameAssets.Instance.GetModuleAsset(tower.modules[0]));
        else slot1.GetComponent<Image>().sprite = defaultSlotSprite;

        if (!String.IsNullOrEmpty(tower.modules[1])) slot2.GetComponent<ModuleSlots>().NewTowerSelected(GameAssets.Instance.GetModuleAsset(tower.modules[1]));
        else slot2.GetComponent<Image>().sprite = defaultSlotSprite;

        if (!String.IsNullOrEmpty(tower.modules[2])) slot3.GetComponent<ModuleSlots>().NewTowerSelected(GameAssets.Instance.GetModuleAsset(tower.modules[2]));
        else slot3.GetComponent<Image>().sprite = defaultSlotSprite;

        if (!String.IsNullOrEmpty(tower.modules[3])) slot4.GetComponent<ModuleSlots>().NewTowerSelected(GameAssets.Instance.GetModuleAsset(tower.modules[3]));
        else slot4.GetComponent<Image>().sprite = defaultSlotSprite;
    }

    private void ShowMenuWithDefaultModuleSlots()
    {
        slot1.GetComponent<Image>().sprite = defaultSlotSprite;
        slot2.GetComponent<Image>().sprite = defaultSlotSprite;
        slot3.GetComponent<Image>().sprite = defaultSlotSprite;
        slot4.GetComponent<Image>().sprite = defaultSlotSprite;
    }
}
