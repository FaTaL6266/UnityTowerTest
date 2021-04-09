using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused;

    public GameObject pauseMenu;
    public GameObject createdPauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            if (gameIsPaused) PauseGame();
            else if (!gameIsPaused) UnpauseGame();
        }
    }

    void PauseGame()
    {
        createdPauseMenu = Instantiate(pauseMenu, GameObject.Find("GameHandler/Background").transform);
        GameHandler.Instance.gameObject.GetComponent<GameSceneWindow>().GamePaused(createdPauseMenu);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        if (createdPauseMenu)
        {
            Destroy(createdPauseMenu);
        }
    }
}