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
    public GameObject DialogueBox;
    public TextMeshProUGUI textComponent;
    public Image imageLeft;
    public Image imageRight;
    public Image imageRight2;
    public GameObject backgroundImage;
    public float textSpeed;
    private Sprite sprite;

    //Used to alter Buttons corresponding to DialogueLine containing options
    private List<Button> dialogueOptions = new List<Button>();
    public Button dialogueOption0;
    public Button dialogueOption1;
    public Button dialogueOption2;

    //Used for logic controlling game
    private int index = 0;
    private string prevChar = "";
    private string prevPos = "";
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
        //var tempColor = imageLeft.color;
        //tempColor.a = 0.5f;
        //imageLeft.color = tempColor;
        /*LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = imageLeft.color;
            c.a = val;
            imageLeft.color = c;
        });*/

        //setting up Options
        dialogueOptions.Add(dialogueOption0);
        dialogueOptions.Add(dialogueOption1);
        dialogueOptions.Add(dialogueOption2);

        dialogueOption0.onClick.AddListener(OnDialogueOption0Click);
        dialogueOption1.onClick.AddListener(OnDialogueOption1Click);
        dialogueOption2.onClick.AddListener(OnDialogueOption2Click);
        

        StartDialogue();
    }

    // Update is called once per framez - //RELOCATE INDEX += 1;
    void Update()
    {   
        print("index: " + index);
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && validInput) - for mobile
        if (Input.GetMouseButtonDown(0) && textComponent.text != lines[index].content)
        {
            StopAllCoroutines();
            textComponent.text = lines[index].content;
        }
        else if (Input.GetMouseButtonDown(0) && index + 1 < lines.Count) {
            //text already loaded in dialoguebox
            textComponent.text = string.Empty;
            index += 1;
            //end of dialogue nodes
            if (index == lines.Count) {
                DialogueBox.SetActive(false);
            } else {
                StartDialogue();
            }   
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

    //runs once every DialogueLine
    void StartDialogue() {
        StartCoroutine(TypeLine());
        StartCoroutine(UpdateCharacterImage());
        // display choices, if any, for this dialogue line
        if (lines[index].options.Count > 0) {
            DisplayChoices();
        } else {
            dialogueOption0.gameObject.SetActive(false); 
            dialogueOption1.gameObject.SetActive(false); 
            dialogueOption2.gameObject.SetActive(false);
        }   
    }

    //precondition: Must have choices available
    private void DisplayChoices() {
        List<Option> options = lines[index].options;
        TextMeshProUGUI txt;
        //defensive check to make sure our UI can support number of choices coming in
        if (options.Count > 3) {
            Debug.LogError("More choices were given than can be supported: " + options.Count);
            foreach (Option option in options) {
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

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach (char c in lines[index].content)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator UpdateCharacterImage() {
        //need to add animation if different character says line
        sprite = Resources.Load<Sprite>("Images/" + lines[index].name + "/" + getEmotion(lines[index]));
        
        //It's a different character saying a line
        if (prevChar != lines[index].name)
        {
            prevChar = lines[index].name;
            PlayExitAnimation();
        } else //when character is the same
        {
            // TODO -> Will have to put blur for image transition
            if (lines[index].position == "L")
            {
                imageLeft.GetComponent<Image>().sprite = sprite;
            } else if (lines[index].position == "R")
            {
                if (prevPosR == 1)
                {
                    imageRight.GetComponent<Image>().sprite = sprite;
                }
                else if (prevPosR == 2)
                {
                    imageRight2.GetComponent<Image>().sprite = sprite;
                }
            }
        }
        yield return null; // Add a short delay if needed
    }

    private void PlayExitAnimation()
    {
        if (prevPos == "L") //previous character on Left - Useful function: setOnComplete(() => function());
        {
            characterExit(imageLeft);
        } else if (prevPos == "R") //previous character on Right
        {
            if (prevPosR == 1)
            {
                characterExit(imageRight);
                
            } else if (prevPosR == 2)
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
        } else if (prevPos == "R") { 
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
        } else if (lines[index].position == "L")
        {
            backgroundImage.transform.LeanMoveLocal(new Vector2(2.5f, 0), 0.3f);
        }
    }

    private void PlayEntryAnimation()
    {
        if (lines[index].position == "L")
        {
            characterEntry(imageLeft);
        } else if (lines[index].position == "R")
        {
            if (prevPosR < 2)
            {
                characterEntry(imageRight2);
                prevPosR = 2;
            } else if (prevPosR == 2)
            {
                characterEntry(imageRight);
                prevPosR = 1;
            }   
        } else
        {
            Debug.Log("No position for image given: error!");
        }
    }

    private void characterEntry(Image image)
    {
        image.GetComponent<Image>().sprite = sprite;
        if (lines[index].position == "L")
        {
            image.transform.LeanMoveLocal(new Vector2(-114, -82), 0.5f);
        } else if (lines[index].position == "R") {
            image.transform.LeanMoveLocal(new Vector2(114, -82), 0.5f);
        }
        image.transform.LeanScale(Vector2.one, 0.5f);
        LeanTween.value(gameObject, 0, 1, 0.5f).setOnUpdate((float val) =>
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
}
