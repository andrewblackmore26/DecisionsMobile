using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageLeft : Stage
{
    public StageLeft(GameObject backgroundImage, GameObject backgroundImage2) : base(backgroundImage, backgroundImage2)
    {
    }

    public override Stage switchSides()
    {
        getBackgroundImage().transform.LeanMoveLocal(new Vector2(1.0f, 0), 0.4f);
        return new StageRight(this.getBackgroundImage(), this.getBackgroundImage2());
    }

    public override Stage transition()
    {
        return null;
    }
}