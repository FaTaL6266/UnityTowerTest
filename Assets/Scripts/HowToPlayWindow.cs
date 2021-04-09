using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class HowToPlayWindow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("MenuButton").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.MainMenu);
    }
}
