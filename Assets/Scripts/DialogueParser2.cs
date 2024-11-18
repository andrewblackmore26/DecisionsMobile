using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.Diagnostics;
using System;

public class DialogueParser2 : MonoBehaviour
{
    public Dictionary<string, List<DialogueLine>> dialogue = new Dictionary<string, List<DialogueLine>>();
    public bool isDialogueLoaded = false;

    void Start()
    {
        string file = "Assets/Data/Dialogue2";
        //string sceneNum = EditorSceneManager.GetActiveScene().name;
        //sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        //file += sceneNum;
        file += ".txt";
        StartCoroutine(LoadDialogues(file));

        //used to print out all lines of content in the chapter
        /*foreach (KeyValuePair<string, List<DialogueLine>> entry in dialogue)
        {
            foreach (DialogueLine line in entry.Value)
            {
                print(line.ToString() + " : " + entry.Key);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator LoadDialogues(string filename)
    {
        string line;
        string key = "";
        StreamReader r = new StreamReader(filename);
        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null || !string.IsNullOrEmpty(line))
                {

                    if (line[0] == '*')
                    {
                        line = line[1..^1];
                        string[] lineData = trimmedArray(line.Split(":"));
                        DialogueLine lineEntry = new DialogueLine(lineData, "background");
                        addEntry(key, lineEntry);
                    }
                    else if (line[0] == '!')
                    {
                        line = line[1..];
                        string[] lineData = trimmedArray(line.Split(":"));
                        DialogueLine lineEntry = new DialogueLine(lineData, "mainCharacter");
                        addEntry(key, lineEntry);
                    }
                    else if (line[0] == '@')
                    {
                        key = line[1..].Trim();
                    }
                    else if (line[0] == '#')
                    {
                        print("GAME OVER SHOULD BE HERE");
                        string[] lineData = trimmedArray(line.Split(":"));
                        DialogueLine lineEntry = new DialogueLine(lineData, "gameOver");
                        addEntry(key, lineEntry);
                    }
                    else
                    {
                        string[] lineData = trimmedArray(line.Split(":"));
                        string lastPart = lineData[^1];

                        //options event detecteds
                        if (Regex.IsMatch(lastPart, "^(o|to|io)[2-4]$"))
                        {
                            int count = int.Parse(lastPart[^1].ToString());
                            string type = lastPart.StartsWith("io") ? "imageOptions" :
                                          lastPart.StartsWith("to") ? "timedOptions" :
                                          "options";
                            DialogueLine lineEntry = new DialogueLine(lineData, type);
                            for (int i = 0; i < count; i++)
                            {
                                string[] optionData = r.ReadLine().Split(":");
                                if (optionData.Length == 1)
                                {
                                    Option option = new Option(optionData[0]);
                                    lineEntry.options.Add(option);
                                }
                                else if (optionData.Length == 2)
                                {
                                    Option option = new Option(optionData[0], optionData[1]);
                                    lineEntry.options.Add(option);
                                }
                                else if (optionData.Length == 3)
                                {
                                    Option option = new Option(optionData[0], optionData[1], optionData[2]);
                                    lineEntry.options.Add(option);
                                }
                            }
                            addEntry(key, lineEntry);
                        }
                        else
                        {
                            DialogueLine lineEntry = new DialogueLine(lineData, "dialogue");
                            addEntry(key, lineEntry);
                        }
                    }
                }
            }
            while (line != null);
            r.Close();
        }
        isDialogueLoaded = true;
        yield return null;
    }

    void addEntry(string key, DialogueLine lineEntry)
    {
        if (dialogue.ContainsKey(key))
        {
            dialogue[key].Add(lineEntry);
        }
        else
        {
            dialogue[key] = new List<DialogueLine>();
            dialogue[key].Add(lineEntry);
        }
    }

    public List<Option> GetOptions(DialogueLine line)
    {
        if (line.options.Count > 0)
        {
            return line.options;
        }
        return null;
    }

    public List<DialogueLine> GetLines(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            return dialogue[key];
        } else
        {
            print("Key does not exist in stored dialogue");
        }
        return null;
    }

    public string[] trimmedArray(string[] lineData)
    {
        for (int i = 0; i < lineData.Length; i++)
        {
            lineData[i] = lineData[i].Trim();
        }
        return lineData;
    }
}
