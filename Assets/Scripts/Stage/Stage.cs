using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public interface Stage
{
    public Stage transition();

    //not yet implemented
    public void shake();
}