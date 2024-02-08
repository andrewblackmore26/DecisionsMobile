using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageLeft : Stage
{
    public StageLeft(GameObject backgroundImage)
    {
        this.backgroundImage = backgroundImage;
    }

    public override Stage switchSides()
    {
        Debug.Log("Moving right");
        backgroundImage.transform.LeanMoveLocal(new Vector2(1.0f, 0), 0.4f);
        return new StageRight(backgroundImage);
    }

    public override Stage transition()
    {
        return null;
    }
}