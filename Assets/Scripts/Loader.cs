using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
        HowToPlay,
        Credits
    }

    public static Action loaderCallbackAction;
    public static bool bDoneLoading;
    public static Scene shareScene;

    public static void Load(Scene scene)
    {
        shareScene = scene;
        bDoneLoading = false;
        loaderCallbackAction = () => { SceneManager.LoadScene(scene.ToString()); };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null)
        {
            bDoneLoading = true;
        }
    }

}
