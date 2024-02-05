using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageLeft : Stage
{
    public GameObject backgroundImage;

    public StageLeft(GameObject backgroundImage)
    {
        this.backgroundImage = backgroundImage;
    }

    public Stage transition()
    {
        Debug.Log("Moving right");
        backgroundImage.transform.LeanMoveLocal(new Vector2(1.5f, 0), 0.3f);
        return new StageRight(backgroundImage);
    }

    public void shake()
    {
        //not yet implemented
    }
}