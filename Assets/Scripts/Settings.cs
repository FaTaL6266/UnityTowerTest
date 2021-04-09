using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static Settings instance;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeScreenSize()
    {
        Camera.main.orthographicSize = 100;
    }
}
