using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Utils
{

    //--------------------------------------------------------------------------------------------------
    //This set of functions are used to find if pointer is over UI Element
    public bool IsPointerOverUIElement(GameObject go)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), go);
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults, GameObject go)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject == go)
            {
                return true;
            }                
        }
        return false;
    }

    // Gets all event system raycast results for the current mouse or touch position
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        // Perform a raycast using the EventSystem
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
    //-------------------------------------------------------------------------------------------------

    // Get new colour of DialogueLine emotion
    public Color getDialogueColour(DialogueLine line)
    {
        Color targetColor;
        ColorUtility.TryParseHtmlString("#00727E", out targetColor);
        switch (line.emotion)
        {
            case 0:
                ColorUtility.TryParseHtmlString("#00727E", out targetColor);
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#00FFF3", out targetColor);
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#383838", out targetColor);
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#FFE100", out targetColor);
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#980300", out targetColor);
                break;
            case 5:
                //emotion = "Suspicious";
                break;
            default:
                Debug.Log("emotion for num not found");
                break;
        }
        return targetColor;
    }

    // Get desired height of DialogueBox
    public float getDialogueHeight(DialogueLine line)
    {
        float extensionAmount = 70.0f;
        if (line.content.Length < 30)
        {
            extensionAmount = 150.0f;
        }
        else if (line.content.Length < 60)
        {
            extensionAmount = 225.0f;
        }
        else if (line.content.Length < 90)
        {
            extensionAmount = 300.0f;
        }
        else
        {
            extensionAmount = 375.0f;
        }

        return extensionAmount;
    }

    // Get emotion of current DialogueLine
    public string getEmotion(DialogueLine line)
    {
        string emotion = null;
        switch (line.getEmotion())
        {
            case 0:
                emotion = "Default";
                break;
            case 1:
                emotion = "Happy";
                break;
            case 2:
                emotion = "Sad";
                break;
            case 3:
                emotion = "Surprised";
                break;
            case 4:
                emotion = "Angry";
                break;
            case 5:
                emotion = "Suspicious";
                break;
            default:
                Debug.Log("emotion for num not found");
                break;
        }
        return emotion;
    }
}