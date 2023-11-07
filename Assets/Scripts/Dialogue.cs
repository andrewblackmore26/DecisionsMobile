using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;

public class Dialogue : MonoBehaviour
{
    public GameObject DialogueBox;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    

    private int index;
    private List<DialogueNode> dialogueNodes;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    Sprite sprite;
    string emotion;
    // Start is called before the first frame updates
    public Button dialogueOption0;
    public Button dialogueOption1;
    public Button dialogueOption2;
    void Start()
    {   
        index = 0;
        textComponent.text = string.Empty;
        string json = File.ReadAllText(@"./Assets/Scripts/Content.json"); 
        dialogueNodes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DialogueNode>>(json);

        //LOAD TEXTURE
        // myTexture = Resources.Load ("Images/Default") as Texture2D;
        // GameObject rawImage = GameObject.Find ("RawImage");
		// rawImage.GetComponent<RawImage>().texture = myTexture;

        //LOAD SPRITE 
		GameObject image = GameObject.Find("CharSprite");
		image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Angry");
        emotion = "Angry";
		
        StartDialogue();
    }

    // Update is called once per framez
    void Update()
    {   
        if (Input.GetMouseButtonDown(0) && index < dialogueNodes.Count) {
            if (textComponent.text == dialogueNodes[index].script) {
                textComponent.text = string.Empty;
                index += 1;
                if (index == dialogueNodes.Count) {
                    //end of dialogue nodes
                    StopAllCoroutines();
                    DialogueBox.SetActive(false);

                    
                } else {
                    StartDialogue();
                }
                
            } else {
                StopAllCoroutines();
                textComponent.text = dialogueNodes[index].script;
            }
            
        }
    }

    void StartDialogue() {
        StartCoroutine(TypeLine());
        StartCoroutine(UpdateCharacterImage());
        // display choices, if any, for this dialogue line
        if (dialogueNodes[index].choices != null) {
            DisplayChoices();
        } else {
            GameObject dialogueChoices = GameObject.Find("DialogueChoices");
            dialogueChoices.SetActive(false);
        }   
        
    }

    private void DisplayChoices() {
        GameObject dialogueChoices = GameObject.Find("DialogueChoices");
        dialogueChoices.SetActive(true);
        List<Choice> currentChoices = dialogueNodes[index].choices;
        Debug.LogError(dialogueNodes[index].choices);
        //defensive check to make sure our UI can support number of choices coming in
        if (currentChoices.Count > choices.Length) {
            Debug.LogError("More choices were given than can be supported" + currentChoices.Count);
            foreach (Choice choice in currentChoices) {
                Debug.LogError(choice.text);
            }
        }

        int choiceIndex = 0;

        // enable and initalize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices) {
            choices[choiceIndex].gameObject.SetActive(true);
            choicesText[choiceIndex].text = choice.text;
            choiceIndex++;
        }

        //go through the remaining choices the UI supports and make sure they're hidden
        // for (int i = choiceIndex; i < choices.Length; i++) {
        //     choices[i].gameObject.SetActive(false);
        // }
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach (char c in dialogueNodes[index].script)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            gameObject.SetActive(false);
        }
    }

    IEnumerator UpdateCharacterImage() {
        if (!(emotion == dialogueNodes[index].emotion)) {
            sprite = Resources.Load<Sprite>("Images/" + dialogueNodes[index].emotion);
            GameObject image = GameObject.Find("CharSprite");
            image.GetComponent<Image>().sprite = sprite;
            emotion = dialogueNodes[index].emotion;
        }
        yield return null; // Add a short delay if needed

        //test 
        // GameObject image = GameObject.Find("CharSprite");
        // image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Happy");
    }
}
