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
    //UI Areas for handling clicks
    public GameObject dialogueArea;
    public GameObject menuArea;

    //User to handle Menu Buttons
    public Button menuButton0;
    public Button menuButton1;

    //Used for loading DialogueLines
    public DialogueParser parser;
    public List<DialogueLine> lines;

    //Used for altering DialogueBox state/text
    public GameObject dialogueBox;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI charName;
    public float textSpeed;

    //Used to control background image transitions, slow pans
    public GameObject backgroundImage;
    public GameObject backgroundImage2;
    private Stage stage;

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
    private ChoicesManager choicesManager;
    private List<Button> dialogueOptions = new List<Button>();
    public Button dialogueOption0;
    public Button dialogueOption1;
    public Button dialogueOption2;

    //Used for logic controlling game
    private Utils Utils;
    private int index = 0;
    private string prevChar = "QQQQQ";
    private string prevPos = "QQQQQ";
    private int prevPosR = 0;
    private Image currEmotion;

    void Start()
    {
        index = 0;
        textComponent.text = string.Empty;

        parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        lines = parser.GetLines("Start");
        parser.GetComponent<DialogueParser>().enabled = false;
        Utils = new Utils();

        //setting up images
        imageLeft.transform.localScale = Vector2.zero;
        imageRight.transform.localScale = Vector2.zero;
        imageRight2.transform.localScale = Vector2.zero;

        //setting up background
        stage = new StageLeft(backgroundImage, backgroundImage2, this, 0.0f, 0.0f);
        
        //setting up choices
        choicesManager = new ChoicesManager(dialogueOption0, dialogueOption1, dialogueOption2, this);
    
        StartDialogue();
    }

    //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && validInput) - for mobile, !EventSystem.current.IsPointerOverGameObject() for pc
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //CloseBox(dialogueBox);
        }
        if (Utils.IsPointerOverUIElement(dialogueArea))
        {

            // If DialogueBox text has not yet loaded
            if (Input.GetMouseButtonDown(0) && textComponent.text != lines[index].content)
            {
                StopAllCoroutines();
                textComponent.text = lines[index].content;
                if (currEmotion) { currEmotion.color = Utils.getDialogueColour(lines[index]); }
            }
            //text already loaded in dialoguebox -> go to next dialogueLine
            else if (Input.GetMouseButtonDown(0) && index + 1 < lines.Count)
            {
                textComponent.text = string.Empty;
                index += 1;
                StartDialogue();
            }
            else if (Input.GetMouseButtonDown(0) && index < lines.Count && lines[index].next != "")
            {           
                string key = lines[index].next;
                index = 0;
                lines = parser.GetLines(key);
                textComponent.text = string.Empty;
                StartDialogue();
            }
        }
        if (Utils.IsPointerOverUIElement(menuArea))
        {
            menuButton0.gameObject.SetActive(true);
            menuButton1.gameObject.SetActive(true);
        } else
        {
            menuButton0.gameObject.SetActive(false);
            menuButton1.gameObject.SetActive(false);
        }
    }

    //runs once every DialogueLine
    void StartDialogue()
    {
        if (lines[index].name == "T")
        {
            //dialogueArea.raycast = false;
            PlayExitAnimation();
            CloseBox(dialogueBox);
            stage.BackgroundTransition(dialogueArea, lines[index]);
        } else
        {
            UpdateCharacterImage();
            StartCoroutine(UpdateDialogueBox());            
        }
        choicesManager.handleChoices(lines[index]);
        prevChar = lines[index].name;
    }


    
    private void UpdateCharacterImage()
    {
        sprite = Resources.Load<Sprite>("Images/" + lines[index].name + "/" + lines[index].position + "/" + Utils.getEmotion(lines[index]));
        dialogueArea.GetComponent<Image>().raycastTarget = false;
        //It's a different character saying a line
        if (prevChar != lines[index].name)
        {
            PlayExitAnimation();
            PlayEntryAnimation();
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
        //yield return null; // Add a short delay if needed.
    }

    private void PlayExitAnimation()
    {
        if (prevPos == "L") //previous character on Left - Useful function: setOnComplete(() => function());
        {
            imageLeft.transform.LeanMoveLocal(new Vector2(-964, -292), 0.5f);
            imageLeft.transform.LeanScale(Vector2.zero, 0.5f);
        }
        else if (prevPos == "R") //previous character on Right
        {
            if (prevPosR == 1)
            {
                imageRight.transform.LeanMoveLocal(new Vector2(971, -292), 0.5f);
                imageRight.transform.LeanScale(Vector2.zero, 0.5f);
            }
            else if (prevPosR == 2)
            {
                imageRight2.transform.LeanMoveLocal(new Vector2(971, -292), 0.5f);
                imageRight2.transform.LeanScale(Vector2.zero, 0.5f);
            }
        }        
        prevPos = lines[index].position;
    }

    private void PlayEntryAnimation()
    {
        if (lines[index].position == "L")
        {
            if (stage is StageRight)
            {
                stage = stage.switchSides();
            }
            characterEntry(imageLeft);
        }
        else if (lines[index].position == "R")
        {
            if (stage is StageLeft)
            {
                stage = stage.switchSides();
            }
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
    }

    private void characterEntry(Image image)
    {
        // reset emotion on entry
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
        Color targetColor = Utils.getDialogueColour(lines[index]);
        // Convert hex color code to Color
        Color startColor = image.color;

        //for when coroutine is stopped early
        currEmotion = image;

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

    //----------------------------------------------------------------------------------------------------------------------------------------------
    //-------------                                        HANDLES DIALOGUE BOX DISPLAY/CONTENT                                         ------------
    //----------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator TypeLine()
    {
        dialogueArea.GetComponent<Image>().raycastTarget = true;
        // Type each character 1 by 1
        foreach (char c in lines[index].content)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        
    }

    IEnumerator UpdateDialogueBox()
    {
        if (prevChar == lines[index].name)
        {
            AdjustBoxForSameSpeaker(dialogueBox);
        }
        else
        {
            CloseBox(dialogueBox);
            yield return new WaitForSeconds(0.4f);
            Utils.RepositionBox(dialogueBox, lines[index]);
            UpdateBottomOfDialogueBox(dialogueBox);
        }
        charName.text = lines[index].name;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(TypeLine());
    }

    void UpdateBottomOfDialogueBox(GameObject image)
    {        
        // GET DESIRED HEIGHT
        float extensionAmount = Utils.getDialogueHeight(lines[index]);

        // Reset height and position to minimode
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector3 currentPosition = rectTransform.localPosition;
        rectTransform.sizeDelta = new Vector2(currentSize.x, 100);
        rectTransform.localPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
        currentSize = rectTransform.sizeDelta;
        currentPosition = rectTransform.localPosition;
        dialogueBox.SetActive(true);

        // Extend the bottom by animating the size change with ease-out
        LeanTween.value(gameObject, currentSize.y, currentSize.y + extensionAmount, 0.4f)
            .setOnUpdate((float value) =>
            {
                rectTransform.sizeDelta = new Vector2(currentSize.x, value);
            })
            .setEase(LeanTweenType.easeOutBack); 

        // Move the object upward to keep the top border in place with ease-out
        LeanTween.value(gameObject, currentPosition.y, currentPosition.y - (extensionAmount / 2.0f), 0.4f)
            .setOnUpdate((float value) =>
            {
                rectTransform.localPosition = new Vector3(currentPosition.x, value, currentPosition.z);
            })
            .setEase(LeanTweenType.easeOutBack); 
    }

    private void AdjustBoxForSameSpeaker(GameObject image)
    {
        float extensionAmount = Utils.getDialogueHeight(lines[index]);
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector3 currentPosition = rectTransform.localPosition;
        // Extend the bottom by animating the size change with ease-out
        LeanTween.value(gameObject, currentSize.y, 100 + extensionAmount, 0.2f)
            .setOnUpdate((float value) =>
            {
                rectTransform.sizeDelta = new Vector2(currentSize.x, value);
            });
        // Move the object upward to keep the top border in place with ease-out
        LeanTween.value(gameObject, currentPosition.y, currentPosition.y + ((currentSize.y - (100 + extensionAmount)) / 2.0f), 0.2f)
            .setOnUpdate((float value) =>
            {
                rectTransform.localPosition = new Vector3(currentPosition.x, value, currentPosition.z);
            }); 
    }

    // Handles dialogueBox closing animation
    private void CloseBox(GameObject box)
    {
        RectTransform rectTransform = box.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector3 currentPosition = rectTransform.localPosition;

        //TO CHANGE
        textComponent.text = "";
        charName.text = "";

        LeanTween.value(gameObject, currentSize.y, 100, 0.15f)
            .setOnUpdate((float value) =>
            {
                rectTransform.sizeDelta = new Vector2(currentSize.x, value);
            });
        LeanTween.value(gameObject, currentPosition.y, currentPosition.y + ((currentSize.y - 100) / 2.0f), 0.15f)
            .setOnUpdate((float value) =>
            {
                rectTransform.localPosition = new Vector3(currentPosition.x, value, currentPosition.z);
            }).setOnComplete(() =>
            {
                box.SetActive(false);
            });
    }

    //coupled with ChoicesManager - enters new path on option click
    public void handleDialogueOptionClick(int num)
    {
        string key = lines[index].options[num].next;
        index = 0;
        lines = parser.GetLines(key);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    //coupled with Stage - goes to next line on background transition
    public void nextLine()
    {
        index++;
        StartDialogue();
    }
}
