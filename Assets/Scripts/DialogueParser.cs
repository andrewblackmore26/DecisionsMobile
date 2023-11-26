using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.Diagnostics;
using System;

public class DialogueParser : MonoBehaviour
{

    struct DialogueLine
    {
        public string name;
        public string content;
        public int emotion;
        public string position;
        public List<Option> options;
        public string next;

        public DialogueLine(string Name, string Content, int Emotion, string Position, string next)
        { 
            name = Name; 
            content = Content; 
            emotion = Emotion; 
            position = Position;
            options = new List<Option>();
            this.next = next;
        }

        public override string ToString()
        {
            return name + " " + content + " " + emotion;
        }
    }

    List<DialogueLine> lines;
    Dictionary<string, List<DialogueLine>> dialogue = new Dictionary<string, List<DialogueLine>>();

    void Start()
    {

        string file = "Assets/Data/Dialogue1";
        string sceneNum = EditorSceneManager.GetActiveScene().name;
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".txt";
        print(file);
        lines = new List<DialogueLine>();

        LoadDialogues(file);
        foreach (KeyValuePair<string, List<DialogueLine>> entry in dialogue)
        {
            //print("NEW KEY: " + entry.Key);
            foreach (DialogueLine line in entry.Value)
            {
                print(line.ToString() + " : " + entry.Key);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadDialogues(string filename)
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
                    string[] lineData = line.Split(';');
                    if (lineData[0] == "Key")
                    {
                        key = lineData[1];
                    }
                    else if (lineData[0] == "Options")
                    {
                        int count = int.Parse(lineData[1]);
                        DialogueLine lineEntry = new DialogueLine(lineData[2], lineData[3], int.Parse(lineData[4]), lineData[5], "");
                        for (int i = 0; i < count; i++)
                        {
                            lineData = r.ReadLine().Split(";");
                            Option option = new Option(lineData[0], lineData[1]);
                            lineEntry.options.Add(option);
                        }
                        addEntry(key, lineEntry);
                    } 
                    else
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3], "");
                        addEntry(key, lineEntry);
                    }
                }
            }
            while (line != null);
            r.Close();
        }
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

    public string GetPosition(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].position;
        }
        return "";
    }

    public string GetName(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].name;
        }
        return "";
    }

    public string GetContent(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].content;
        }
        return "";
    }

    public int GetEmotion(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].emotion;
        }
        return 0;
    }

    /*public string[] GetOptions(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].options;
        }
        return new string[0];
    }*/
}
