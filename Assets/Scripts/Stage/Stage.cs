using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public abstract class Stage
{
    public GameObject backgroundImage;
    public GameObject backgroundImage2;

    public abstract Stage transition();
    public abstract Stage switchSides();

    public void shake()
    {
        //not yet implemented
    }
}