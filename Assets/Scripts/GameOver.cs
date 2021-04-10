using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GameOver : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("RestartButton").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.GameScene);
        transform.Find("MainMenuButton").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.MainMenu);
        transform.Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
        gameObject.SetActive(false);
    }
}
