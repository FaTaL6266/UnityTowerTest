using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class HowToPlayWindow : MonoBehaviour
{
    private Image activeScreenImage;
    [SerializeField] private Sprite[] howToPlayImages;
    private int imageIndex = 0;

    private Button_UI nextButton;
    private Button_UI backButton;

    private void Awake()
    {
        activeScreenImage = GetComponent<Image>();

        nextButton = transform.Find("NextButton").GetComponent<Button_UI>();
        backButton = transform.Find("BackButton").GetComponent<Button_UI>();

        transform.Find("MenuButton").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.MainMenu);
        nextButton.ClickFunc = () => { if (imageIndex < howToPlayImages.Length - 1) ChangeImage(1); };
        backButton.ClickFunc = () => { if (imageIndex > 0) ChangeImage(-1); };
    }

    private void ChangeImage(int movePage)
    {
        imageIndex += movePage;
        activeScreenImage.sprite = howToPlayImages[imageIndex];

        if (imageIndex == 0) backButton.enabled = false;
        else
        {
            backButton.enabled = true;
        }

        if (imageIndex == howToPlayImages.Length - 1) nextButton.enabled = false;
        else nextButton.enabled = true;
    }
}
