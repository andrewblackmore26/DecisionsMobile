using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;

public class ChoicesManager
{
    private Dialogue dialogue;
    private List<Button> dialogueOptions = new List<Button>();
    private Button dialogueOption0;
    private Button dialogueOption1;
    private Button dialogueOption2;

    public ChoicesManager(Button dialogueOption0, Button dialogueOption1, Button dialogueOption2, Dialogue dialogue)
    {
        this.dialogueOption0 = dialogueOption0;
        this.dialogueOption1 = dialogueOption1;
        this.dialogueOption2 = dialogueOption2;
        this.dialogue = dialogue;
        setupChoicesManager();
    }

    public void handleChoices(DialogueLine line)
    {
        if (line.options.Count > 0)
        {
            DisplayChoices(line);
        }
        else
        {
            dialogueOption0.gameObject.SetActive(false);
            dialogueOption1.gameObject.SetActive(false);
            dialogueOption2.gameObject.SetActive(false);
        }
    }

    private void DisplayChoices(DialogueLine line)
    {
        List<Option> options = line.options;
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
            txt.text = line.options[i].content;
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

    private void OnDialogueOption0Click()
    {
        dialogue.handleDialogueOptionClick(0);
    }

    private void OnDialogueOption1Click()
    {
        dialogue.handleDialogueOptionClick(1);
    }

    private void OnDialogueOption2Click()
    {
        dialogue.handleDialogueOptionClick(2);
    }

    private void setupChoicesManager()
    {
        dialogueOption0.onClick.AddListener(OnDialogueOption0Click);
        dialogueOption1.onClick.AddListener(OnDialogueOption1Click);
        dialogueOption2.onClick.AddListener(OnDialogueOption2Click);
        dialogueOptions.Add(dialogueOption0);
        dialogueOptions.Add(dialogueOption1);
        dialogueOptions.Add(dialogueOption2);
    }


}