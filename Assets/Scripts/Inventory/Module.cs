using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module
{
    public enum ModuleType
    {
        Health,
        PhysicalDamage,
        FireDamage,
        FireRate,
        FireResistance,
        PhysicalResistance
    }

    public ModuleType moduleType;
    public int amount;
}
