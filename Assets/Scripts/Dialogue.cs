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
    public GameObject image;
    public float textSpeed;
    private Sprite sprite;

    //Used to alter Buttons corresponding to DialogueLine containing options
    private List<Button> dialogueOptions = new List<Button>();
    public Button dialogueOption0;
    public Button dialogueOption1;
    public Button dialogueOption2;

    //Used for logic controlling game
    private int index;
    private string prevChar;

    void Start()
    {
        index = 0;
        textComponent.text = string.Empty;

        //why do i need this line?
        parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        lines = parser.GetLines("Start");
        parser.GetComponent<DialogueParser>().enabled = false;

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

    //CURRENTLY UNUSED
    void NextLine() {
        if (index < lines.Count - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            //WTF does this do
            gameObject.SetActive(false);
        }
    }

    IEnumerator UpdateCharacterImage() {
        //need to add animation if different character says line
        sprite = Resources.Load<Sprite>("Images/" + lines[index].name + "/" + getEmotion(lines[index]));
        image.GetComponent<Image>().sprite = sprite;
        //image.transform.LeanMoveLocal(new Vector2(270, 40), 1);
        yield return null; // Add a short delay if needed
    }

    private void OnDialogueOption0Click()
    {
        string key = lines[index].options[0].next;
        index = 0;
        Debug.Log("Button 0 clicked!");
        lines = parser.GetLines(key);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void OnDialogueOption1Click()
    {
        string key = lines[index].options[1].next;
        index = 0;
        Debug.Log("Button 1 clicked!");
        lines = parser.GetLines(key);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    private void OnDialogueOption2Click()
    {
        index = 0;
        Debug.Log("Button 2 clicked!");
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
