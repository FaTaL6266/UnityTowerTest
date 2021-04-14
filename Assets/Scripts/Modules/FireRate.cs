using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRate : MonoBehaviour
{
    public float GetModifier()
    {
        switch (gameObject.GetComponent<DragDrop>().moduleRarity)
        {
            case ModuleRarity.COMMON:    return 0.2f;
            case ModuleRarity.UNCOMMON:  return 0.4f;
            case ModuleRarity.RARE:      return 0.6f;
            case ModuleRarity.EXOTIC:    return 0.8f;
            case ModuleRarity.LEGENDARY: return 1.0f;
        }
        return 0f;
    }
}
