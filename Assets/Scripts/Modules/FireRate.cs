using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRate : MonoBehaviour
{
    public float GetModifier()
    {
        switch (gameObject.GetComponent<DragDrop>().moduleRarity)
        {
            case ModuleRarity.COMMON:    return 10f;
            case ModuleRarity.UNCOMMON:  return 15f;
            case ModuleRarity.RARE:      return 20f;
            case ModuleRarity.EXOTIC:    return 25f;
            case ModuleRarity.LEGENDARY: return 30f;
        }
        return 0f;
    }
}
