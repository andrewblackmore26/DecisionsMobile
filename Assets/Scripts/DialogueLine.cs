using System.Collections.Generic;

public class DialogueLine
{
    public string name;
    public string content;
    public int emotion;
    public string position;
    public List<Option> options;
    public string next;

    //new additions
    public string type;
    public string panDirection;
    public int panLength;
    public string fadeColour;

    public string charEffect;
    public bool timed;
    public bool image;

    public int power0;
    public int power1;
    public int power2;

    // might be better to make this a super class, make the types child classes.
    public DialogueLine(string[] lineData, string Type)
    {
        options = new List<Option>();
        type = Type;
        if (type == "background")
        {
            InitialiseBackgroundEvent(lineData);
        } else if (type == "mainCharacter")
        {
            name = lineData[0];
        } else if (type == "gameOver")
        {
            name = "N";
            content = "GAME OVER PUSSY";
        } else if (type == "options" || type == "timedOptions" || type == "imageOptions")
        {
            InitialiseOptionsEvent(lineData);
        } else
        {
            InitialiseDialogueEvent(lineData);
        }
    }

    public DialogueLine(string Name, string Content, int Emotion, string Position, string next)
    {
        name = Name;
        content = Content;
        emotion = Emotion;
        position = Position;        
        options = new List<Option>();
        this.next = next;
    }

    // new function
    public void InitialiseDialogueEvent(string[] lineData)
    {
        string[] charDetails = lineData[0].Split(";");
        name = charDetails[0];
        if (charDetails.Length > 1)
        {
            emotion = int.Parse(charDetails[1]);
        }       
        if (charDetails.Length > 2)
        {
            if (char.IsDigit(charDetails[2][0]))
            {
                string[] powerBuff = lineData[2].Split("-");
                switch (powerBuff[0][0])
                {
                    case '0':
                        power0 = int.Parse(powerBuff[1]); break;
                    case '1':
                        power1 = int.Parse(powerBuff[1]); break;
                    case '2':
                        power2 = int.Parse(powerBuff[1]); break;
                }
            }
            charEffect = charDetails[2];
        }
        content = lineData[1];
        if (lineData.Length > 2)
        {
            next = lineData[2];
        }
    }

    // new function
    public void InitialiseOptionsEvent(string[] lineData)
    {
        string[] charDetails = lineData[0].Split(";");
        name = charDetails[0];
        if (charDetails.Length > 1)
        {
            emotion = int.Parse(charDetails[1].ToString());
        }
        if (charDetails.Length > 2)
        {
            charEffect = charDetails[2];
        }
        content = lineData[1];

    }

    // new functions
    public void InitialiseBackgroundEvent(string[] lineData)
    {
        name = lineData[0];
        if (lineData.Length > 1)
        {
            if (lineData[1][0] == 'r')
            {
                panDirection = "right";
                panLength = lineData[1][1]; 
            } else if (lineData[1][0] == 'l')
            {
                panDirection = "left";
                panLength = lineData[1][1];
            } else if (lineData[1][0] == 'u')
            {
                panDirection = "up";
                panLength = lineData[1][1];
            } else if (lineData[1][0] == 'd')
            {
                panDirection = "down";
                panLength = lineData[1][1];
            }
            if (lineData[1][^1] == 'w')
            {
                fadeColour = "white";
            } else if (lineData[1][^1] == 'b')
            {
                fadeColour = "black";
            } else
            {
                fadeColour = "none";
            }
        }
    }

    public int getEmotion()
    {
        return emotion;
    }

    public override string ToString()
    {
        return "name: " + name + " content: " + content + " emotion(int): " + emotion;
    }
}