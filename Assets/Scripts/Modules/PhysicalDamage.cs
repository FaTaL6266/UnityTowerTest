using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : MonoBehaviour
{
    public float GetModifier()
    {
        switch (gameObject.GetComponent<DragDrop>().moduleRarity)
        {
            case ModuleRarity.COMMON:    return 2f;
            case ModuleRarity.UNCOMMON:  return 4f;
            case ModuleRarity.RARE:      return 6f;
            case ModuleRarity.EXOTIC:    return 8f;
            case ModuleRarity.LEGENDARY: return 10f;
        }
        return 0f;
    }
}
