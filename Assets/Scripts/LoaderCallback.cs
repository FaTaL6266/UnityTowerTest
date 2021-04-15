using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool firstUpdate = true;
    private TextMeshProUGUI continueText;
    private Image enemyImage;
    private TextMeshProUGUI enemyDescription;

    private void Awake()
    {
        continueText = transform.Find("ContinueText").GetComponent<TextMeshProUGUI>();
        enemyImage = transform.Find("EnemyImage").GetComponent<Image>();
        enemyDescription = transform.Find("EnemyDescription").GetComponent<TextMeshProUGUI>();
        SetEnemyDetails();
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

    private void SetEnemyDetails()
    {
        int randomEnemy = Random.Range(0, GameAssets.Instance.enemyLoadings.Length);
        enemyImage.sprite = GameAssets.Instance.enemyLoadings[randomEnemy].enemyImage;
        enemyDescription.text = GameAssets.Instance.enemyLoadings[randomEnemy].enemyDescription;
    }
}
