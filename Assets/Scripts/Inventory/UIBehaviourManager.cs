using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviourManager : MonoBehaviour
{
    public static event Action<Tower> OnTowerSelected;
    public static event Action OnModuleDrag;

    public void NewTowerSelected(Tower tower)
    {
        OnTowerSelected?.Invoke(tower);
    }

    public void ModuleDragging()
    {
        OnModuleDrag?.Invoke();
    }
}
