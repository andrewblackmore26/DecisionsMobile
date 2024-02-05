using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageRight : Stage
{
    public GameObject backgroundImage;

    public StageRight(GameObject backgroundImage) 
    {
        this.backgroundImage = backgroundImage;
    }

    public Stage transition()
    {
        Debug.Log("Moving left");
        backgroundImage.transform.LeanMoveLocal(new Vector2(2.5f, 0), 0.3f);
        return new StageLeft(backgroundImage);
    }

    public void shake()
    {
        //not yet implemented
    }
}