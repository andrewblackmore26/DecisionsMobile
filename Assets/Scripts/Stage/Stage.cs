using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public abstract class Stage
{
    private GameObject backgroundImage;
    private GameObject backgroundImage2;
    private Dialogue dialogue;

    public abstract Stage transition();
    public abstract Stage switchSides();

    public Stage(GameObject backgroundImage, GameObject backgroundImage2, Dialogue dialogue)
    {
        this.backgroundImage = backgroundImage;
        this.backgroundImage2 = backgroundImage2;
        this.dialogue = dialogue;
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
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
        currentPosition = transform.localPosition;

        Sprite newSprite = Resources.Load<Sprite>("Images/Background/UndergroundMarket");
        backgroundImage2.GetComponent<SpriteRenderer>().sprite = newSprite;
        float fadeDuration = 0.5f;
        LeanTween.alpha(backgroundImage, 0.0f, fadeDuration).setOnComplete(() =>
        {
            area.GetComponent<Image>().raycastTarget = true;
            rearrangeBackgrounds(newSprite);
            dialogue.nextLine();
        });
    }

    private void rearrangeBackgrounds(Sprite newSprite)
    {
        //Gets component that controls position of BG2
        Transform transform2 = backgroundImage2.GetComponent<Transform>();
        //Gets the current values of the BG2 position - x,y,z
        Vector3 currentPosition = transform2.localPosition;
        //Gets component to control BG position
        Transform transform = backgroundImage.GetComponent<Transform>();
        //Put new values for the BG position -> it will now be repositioned accordingly
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, 0);
        //Put BG back onscreen, which will cover BG2
        backgroundImage.GetComponent<SpriteRenderer>().sprite = newSprite;
        LeanTween.alpha(backgroundImage, 1.0f, 0.0f);
    }

    public GameObject getBackgroundImage() {  return backgroundImage; }
    public void setBackgroundImage(GameObject bg) { backgroundImage = bg; }
    public GameObject getBackgroundImage2() {  return backgroundImage2; }
    public void setBackgroundImage2(GameObject bg) { backgroundImage2 = bg; }
    public Dialogue getDialogue() { return dialogue; }
}