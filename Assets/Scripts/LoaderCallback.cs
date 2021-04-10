using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoaderCallback : MonoBehaviour
{
    private bool firstUpdate = true;
    private TextMeshProUGUI continueText;

    private void Awake()
    {
        continueText = transform.Find("ContinueText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            Loader.LoaderCallback();
        }
        else if (Loader.bDoneLoading == true && Loader.shareScene == Loader.Scene.GameScene)
        {
            RefreshLoadingScene();

            if (Input.anyKey)
            {
                Loader.loaderCallbackAction();
                Loader.loaderCallbackAction = null;
            }
        }
        else
        {
            Loader.loaderCallbackAction();
            Loader.loaderCallbackAction = null;
        }
    }

    private void RefreshLoadingScene()
    {
        continueText.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
    }
}
