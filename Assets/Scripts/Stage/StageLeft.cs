using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class StageLeft : Stage
{
    public StageLeft(GameObject backgroundImage, GameObject backgroundImage2, Dialogue dialogue, float x, float y) : base(backgroundImage, backgroundImage2, dialogue, x, y)
    {
    }

    public override Stage switchSides()
    {
        getBackgroundImage().transform.LeanMoveLocal(new Vector2(this.getX() - 1.0f, 0), 0.3f);
        return new StageRight(this.getBackgroundImage(), this.getBackgroundImage2(), getDialogue(), getX(), getY());
    }

    public override Stage transition()
    {
        return null;
    }
}