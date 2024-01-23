using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Utils
{

    public bool IsMouseOverUI(GameObject area)
    {
        
        // Check if the UI element is your specified UI area
        // You can use yourUIArea's Collider, RectTransform, or any other property
        // Example: Collider collider = yourUIArea.GetComponent<Collider>();
        // Replace "Collider" with the appropriate component type

        // Example using BoxCollider
        Collider collider = area.GetComponent<BoxCollider>();

        if (collider != null)
        {

            // Check if the mouse position is within the bounds of the collider
            return collider.bounds.Contains(Input.mousePosition);
        }


        return false;
    }

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