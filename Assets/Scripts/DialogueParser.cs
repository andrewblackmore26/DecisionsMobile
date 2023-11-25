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
        public string[] options;

        public DialogueLine(string Name, string Content, int Emotion, string Position, string next)
        {
            name = Name;
            content = Content;
            emotion = Emotion;
            position = Position;
            options = new string[0];
        }

        public DialogueLine(string Name, string Content, int Emotion, string Position, string Options, string next)
        { 
            name = Name; 
            content = Content; 
            emotion = Emotion; 
            position = Position;

        }
    }

    List<DialogueLine> lines;

    void Start()
    {

        string file = "Assets/Data/Dialogue";
        string sceneNum = EditorSceneManager.GetActiveScene().name;
        System.Diagnostics.Debug.WriteLine(sceneNum);
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".txt";
        print(file);
        lines = new List<DialogueLine>();

        LoadDialogue(file);
        foreach (DialogueLine line in lines)
        {
            print(line.name + " " + line.content + " " + line.position);
        };
    }

    // Update is called once per frames
    void Update()
    {

    }

    void LoadDialogue(string filename)
    {
        string line;
        StreamReader r = new StreamReader(filename);
        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null || !string.IsNullOrEmpty(line))
                {
                    string[] lineData = line.Split(';');
                    if (lineData[0] == "Player")
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], "", 0, "");
                        lineEntry.options = new string[lineData.Length - 1];
                        for (int i = 1; i < lineData.Length; i++)
                        {
                            lineEntry.options[i - 1] = lineData[i];
                        }
                        lines.Add(lineEntry);
                    }
                    else
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3]);
                        lines.Add(lineEntry);
                    }
                }
            }
            while (line != null);
            r.Close();
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

    public string[] GetOptions(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].options;
        }
        return new string[0];
    }
}
