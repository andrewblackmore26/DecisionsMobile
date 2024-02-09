using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageRight : Stage
{
    public StageRight(GameObject backgroundImage) 
    {
        this.backgroundImage = backgroundImage;
    }

    public override Stage switchSides()
    {
        backgroundImage.transform.LeanMoveLocal(new Vector2(3.0f, 0), 0.4f);
        return new StageLeft(backgroundImage);
    }

    public override Stage transition()
    {
        return null;
    }
}