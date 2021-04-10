using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameSceneWindow : MonoBehaviour
{
    public void GamePaused(GameObject pauseMenu)
    {
        pauseMenu.transform.Find("ResumeButton").GetComponent<Button_UI>().ClickFunc = () => gameObject.GetComponent<PauseController>().UnpauseGame();
        //pauseMenu.transform.Find("SettingsButton").GetComponent<Button_UI>().ClickFunc = () => Instantiate();
        pauseMenu.transform.Find("MainMenuButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            GameHandler.Instance.gameObject.GetComponent<PauseController>().UnpauseGame();
            Loader.Load(Loader.Scene.MainMenu);
        };
        pauseMenu.transform.Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
    }

}
