using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public abstract class Stage
{
    private GameObject backgroundImage;
    private GameObject backgroundImage2;

    public abstract Stage transition();
    public abstract Stage switchSides();

    public Stage(GameObject backgroundImage, GameObject backgroundImage2)
    {
        this.backgroundImage = backgroundImage;
        this.backgroundImage2 = backgroundImage2;
        this.currBg = 1;
    }

    public void shake()
    {
        //not yet implemented
    }

    public void transitions(GameObject bg)
    {
        float fadeDuration = 0.3f;
        LeanTween.alpha(backgroundImage, 0.0f, fadeDuration);
    }


    public void BackgroundTransition(GameObject area)
    {
        Transform transform = getBackgroundImage().GetComponent<Transform>();
        Vector3 currentPosition = transform.localPosition;

        Sprite newSprite = Resources.Load<Sprite>("Images/Background/UndergroundMarket");
        backgroundImage2.GetComponent<SpriteRenderer>().sprite = newSprite;
        float fadeDuration = 0.5f;
        LeanTween.alpha(backgroundImage, 0.0f, fadeDuration).setOnComplete(() =>
        {
            area.SetActive(true);
        });
    }

    public GameObject getBackgroundImage() {  return backgroundImage; }
    public void setBackgroundImage(GameObject bg) { backgroundImage = bg; }
    public GameObject getBackgroundImage2() {  return backgroundImage2; }
    public void setBackgroundImage2(GameObject bg) { backgroundImage2 = bg; }
}