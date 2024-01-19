using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{
    //Used for loading DialogueLines
    public DialogueParser parser;
    public List<DialogueLine> lines;

    //Used for altering DialogueBox state/text
    public GameObject dialogueBox;
    public TextMeshProUGUI textComponent;
    
    public GameObject dialogueBox2;
    public TextMeshProUGUI textComponent2;

    public float textSpeed;

    //Used to control background image transitions, slow pans
    public GameObject backgroundImage;
    public GameObject backgroundImage2;

    //Used for handling image transitions/transparency/movement
    public Image imageLeft;
    public Image charLeft;
    public Image emotionLeft;

    public Image imageRight;
    public Image charRight;
    public Image emotionRight;

    public Image imageRight2;
    public Image charRight2;
    public Image emotionRight2;

    private Sprite sprite;
    public float transitionTime = 0.2f;

    //Used to alter Buttons corresponding to DialogueLine containing options
    private List<Button> dialogueOptions = new List<Button>();
    public Button dialogueOption0;
    public Button dialogueOption1;
    public Button dialogueOption2;

    //Used for logic controlling game
    private int index = 0;
    private string prevChar = "";
    private string prevPos = "";
    private int prevBox = 0;
    private int prevPosR = 0;

    void Start()
    {
        index = 0;
        textComponent.text = string.Empty;

        parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        lines = parser.GetLines("Start");
        parser.GetComponent<DialogueParser>().enabled = false;

        //setting up images
        imageLeft.transform.localScale = Vector2.zero;
        imageRight.transform.localScale = Vector2.zero;
        imageRight2.transform.localScale = Vector2.zero;

        //setting up Options
        dialogueOptions.Add(dialogueOption0);
        dialogueOptions.Add(dialogueOption1);
        dialogueOptions.Add(dialogueOption2);

        dialogueOption0.onClick.AddListener(OnDialogueOption0Click);
        dialogueOption1.onClick.AddListener(OnDialogueOption1Click);
        dialogueOption2.onClick.AddListener(OnDialogueOption2Click);

        StartDialogue();
    }

    //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && validInput) - for mobile, !EventSystem.current.IsPointerOverGameObject() for pc
    void Update()
    {
        // If DialogueBox text has not yet loaded
        if (Input.GetMouseButtonDown(0) && textComponent.text != lines[index].content)
        {
            StopAllCoroutines();
            textComponent.text = lines[index].content;
        }
        //text already loaded in dialoguebox -> go to next dialogueLine
        else if (Input.GetMouseButtonDown(0) && index + 1 < lines.Count)
        {

            textComponent.text = string.Empty;
            index += 1;
            //end of dialogue nodes
            if (index == lines.Count)
            {
                dialogueBox.SetActive(false); // might be unused
            }
            else
            {
                StartDialogue();
            }
        }
        //for option clicks (i think)
        else if (Input.GetMouseButtonDown(0) && index < lines.Count && lines[index].next != "")
        {
            string key = lines[index].next;
            index = 0;
            lines = parser.GetLines(key);
            textComponent.text = string.Empty;
            StartDialogue();
        }
    }

    //runs once every DialogueLine
    void StartDialogue()
    {
        
        StartCoroutine(UpdateCharacterImage());
        StartCoroutine(UpdateDialogueBox());
        StartCoroutine(TypeLine()); // TO BE PLACED ELSEWHERE
        // display choices, if any, for this dialogue line
        if (lines[index].options.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            dialogueOption0.gameObject.SetActive(false);
            dialogueOption1.gameObject.SetActive(false);
            dialogueOption2.gameObject.SetActive(false);
        }
    }

    //precondition: Must have choices available
    private void DisplayChoices()
    {
        List<Option> options = lines[index].options;
        TextMeshProUGUI txt;
        //defensive check to make sure our UI can support number of choices coming in
        if (options.Count > 3)
        {
            Debug.LogError("More choices were given than can be supported: " + options.Count);
            foreach (Option option in options)
            {
                Debug.LogError(option.content);
            }
        }

        int count = 0;
        // enable and initalize the choices up to the amount of choices for this line of dialogue
        for (int i = 0; i < options.Count; i++)
        {
            txt = dialogueOptions[i].GetComponentInChildren<TextMeshProUGUI>();
            txt.text = lines[index].options[i].content;
            dialogueOptions[i].gameObject.SetActive(true);
            count++;
        }

        // go through the remaining choices the UI supports and make sure they're hidden
        while (count < 3)
        {
            dialogueOptions[count].gameObject.SetActive(false);
            count++;
        }
    }

    IEnumerator UpdateCharacterImage()
    {
        sprite = Resources.Load<Sprite>("Images/" + lines[index].name + "/" + lines[index].position + "/" + getEmotion(lines[index]));

        //It's a different character saying a line
        if (prevChar != lines[index].name)
        {
            prevChar = lines[index].name;
            PlayExitAnimation();
        }
        else //when character is the same
        {
            if (lines[index].position == "L")
            {
                StartCoroutine(ChangeColor(emotionLeft));
                StartCoroutine(ChangeChar(charLeft));
            }
            else if (lines[index].position == "R")
            {
                if (prevPosR == 1)
                {
                    StartCoroutine(ChangeColor(emotionRight));
                    StartCoroutine(ChangeChar(charLeft));
                }
                else if (prevPosR == 2)
                {
                    StartCoroutine(ChangeColor(emotionRight2));
                    StartCoroutine(ChangeChar(charRight2));
                }
            }
        }
        yield return null; // Add a short delay if needed.
    }

    private void PlayExitAnimation()
    {
        if (prevPos == "L") //previous character on Left - Useful function: setOnComplete(() => function());
        {
            characterExit(imageLeft);
        }
        else if (prevPos == "R") //previous character on Right
        {
            if (prevPosR == 1)
            {
                characterExit(imageRight);

            }
            else if (prevPosR == 2)
            {
                characterExit(imageRight2);
            }
        }
        PlayEntryAnimation();
        prevPos = lines[index].position;
    }

    private void characterExit(Image image)
    {
        if (prevPos == "L")
        {
            image.transform.LeanMoveLocal(new Vector2(-964, -292), 0.5f);
        }
        else if (prevPos == "R")
        {
            image.transform.LeanMoveLocal(new Vector2(971, -292), 0.5f);
        }
        image.transform.LeanScale(Vector2.zero, 0.5f);
        LeanTween.value(gameObject, 1, 0f, 0.5f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        });
        if (lines[index].position == "R") // Move backgroundImage towards new speaker
        {
            backgroundImage.transform.LeanMoveLocal(new Vector2(1.5f, 0), 0.3f);
        }
        else if (lines[index].position == "L")
        {
            backgroundImage.transform.LeanMoveLocal(new Vector2(2.5f, 0), 0.3f);
        }
    }

    private void PlayEntryAnimation()
    {
        if (lines[index].position == "L")
        {
            characterEntry(imageLeft);
        }
        else if (lines[index].position == "R")
        {
            if (prevPosR < 2)
            {
                characterEntry(imageRight2);
                prevPosR = 2;
            }
            else if (prevPosR == 2)
            {
                characterEntry(imageRight);
                prevPosR = 1;
            }
        }
        else
        {
            Debug.Log("No position for image given: error!");
        }
    }

    private void characterEntry(Image image)
    {
        //reset emotion on entry
        Color targetColor;
        ColorUtility.TryParseHtmlString("#00727E", out targetColor);
        Sprite def = Resources.Load<Sprite>("Images/" + lines[index].name + "/" + lines[index].position + "/Default");

        if (lines[index].position == "L")
        {
            charLeft.GetComponent<Image>().sprite = def;
            emotionLeft.color = targetColor;
            image.transform.LeanMoveLocal(new Vector2(-150, 75), 0.5f).setOnComplete(() =>
            {
                StartCoroutine(ChangeColor(emotionLeft));
                StartCoroutine(ChangeChar(charLeft));
            });
        }
        else if (lines[index].position == "R")
        {
            if (prevPosR < 2)
            {
                charRight2.GetComponent<Image>().sprite = def;
                emotionRight2.color = targetColor;
                image.transform.LeanMoveLocal(new Vector2(150, 75), 0.5f).setOnComplete(() =>
                {
                    StartCoroutine(ChangeColor(emotionRight2));
                    StartCoroutine(ChangeChar(charRight2));
                });
            }
            else if (prevPosR == 2)
            {
                charRight.GetComponent<Image>().sprite = def;
                emotionRight.color = targetColor;
                image.transform.LeanMoveLocal(new Vector2(150, 75), 0.5f).setOnComplete(() =>
                {
                    StartCoroutine(ChangeColor(emotionRight));
                    StartCoroutine(ChangeChar(charRight));
                });
            }
        }
        image.transform.LeanScale(Vector2.one, 0.5f);
        //FADE ANIMATION - CURRENTLY NOT WORKING
        LeanTween.value(gameObject, 0, 1, 1.5f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        });
    }

    private void OnDialogueOption0Click()
    {
        handleDialogueOptionClick(0);
    }

    private void OnDialogueOption1Click()
    {
        handleDialogueOptionClick(1);
    }

    private void OnDialogueOption2Click()
    {
        handleDialogueOptionClick(2);
    }

    private void handleDialogueOptionClick(int num)
    {
        string key = lines[index].options[num].next;
        index = 0;
        Debug.Log("Button " + num + " clicked!");
        lines = parser.GetLines(key);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    //ADD BLUR TO TRANSITION
    IEnumerator ChangeChar(Image image)
    {
        yield return new WaitForSeconds(0.1f);
        image.GetComponent<Image>().sprite = sprite;
        yield break;
    }
    IEnumerator ChangeColor(Image image)
    {
        Color targetColor;
        ColorUtility.TryParseHtmlString("#00727E", out targetColor);
        switch (lines[index].emotion)
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
                print("emotion for num not found");
                break;
        }
        // Convert hex color code to Color
        Color startColor = image.color;

        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            // Calculate the interpolation factor (t) between 0 and 1 based on elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            // Use Color.Lerp to interpolate between the startColor and targetColor
            image.color = Color.Lerp(startColor, targetColor, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
        // Ensure the final color is exactly the target color
        image.color = targetColor;
    }

    private string getEmotion(DialogueLine line)
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
                print("emotion for num not found");
                break;
        }
        return emotion;
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        print(lines[index].content.Length);
        foreach (char c in lines[index].content)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator UpdateDialogueBox()
    {
        RectTransform rectTransform = dialogueBox.GetComponent<RectTransform>();
        Vector3 currentPosition = rectTransform.localPosition;
        if (lines[index].position == "L")
        {
            dialogueBox.transform.LeanMoveLocal(new Vector2(-70, currentPosition.y), 0.0f);
            UpdateBottomOfDialogueBox(dialogueBox);
        } else if (lines[index].position == "R")
        {
            dialogueBox.transform.LeanMoveLocal(new Vector2(70, currentPosition.y), 0.0f);
            UpdateBottomOfDialogueBox(dialogueBox);
        } else if (lines[index].position == "N")
        {
            //FOR NARRATOR
        }
        yield return null;
    }

    void UpdateBottomOfDialogueBox(GameObject image)
    {
        // GET DESIRED HEIGHT
        float extensionAmount = 70.0f;
        if (lines[index].content.Length < 30)
        {
            extensionAmount = 150.0f;
        }
        else if (lines[index].content.Length < 60)
        {
            extensionAmount = 225.0f;
        }
        else if (lines[index].content.Length < 90)
        {
            extensionAmount = 300.0f;
        }
        else
        {
            extensionAmount = 375.0f;
        }

        /*if (prevChar == lines[index].name)
        {
            AdjustBoxForSameSpeaker(image, extensionAmount);
        }
        else
        {
            AdjustBoxForDifferentSpeaker(GameObject image);
        }*/

        // Reset height to minimode
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector3 currentPosition = rectTransform.localPosition;
        rectTransform.sizeDelta = new Vector2(currentSize.x, 100);
        rectTransform.localPosition = new Vector3(currentPosition.x, 120, currentPosition.z);
        currentSize = rectTransform.sizeDelta;
        currentPosition = rectTransform.localPosition;

        // Extend the bottom by animating the size change with ease-out
        LeanTween.value(gameObject, currentSize.y, currentSize.y + extensionAmount, 0.3f)
            .setOnUpdate((float value) =>
            {
                rectTransform.sizeDelta = new Vector2(currentSize.x, value);
            })
            .setEase(LeanTweenType.easeOutBack); // You can change the ease type

        // Move the object upward to keep the top border in place with ease-out
        LeanTween.value(gameObject, currentPosition.y, currentPosition.y - (extensionAmount / 2.0f), 0.3f)
            .setOnUpdate((float value) =>
            {
                rectTransform.localPosition = new Vector3(currentPosition.x, value, currentPosition.z);
            })
            .setEase(LeanTweenType.easeOutBack); // You can change the ease type
    }

    private void AdjustBoxForSameSpeaker(GameObject image, float extensionAmount)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector3 currentPosition = rectTransform.localPosition;
        // Extend the bottom by animating the size change with ease-out
        LeanTween.value(gameObject, currentSize.y, 100 + extensionAmount, 0.3f)
            .setOnUpdate((float value) =>
            {
                rectTransform.sizeDelta = new Vector2(currentSize.x, value);
            }); // You can change the ease type

        // Move the object upward to keep the top border in place with ease-out
        LeanTween.value(gameObject, currentPosition.y, 120 - (extensionAmount / 2.0f), 0.3f)
            .setOnUpdate((float value) =>
            {
                rectTransform.localPosition = new Vector3(currentPosition.x, value, currentPosition.z);
            }); // You can change the ease type
    }
}
