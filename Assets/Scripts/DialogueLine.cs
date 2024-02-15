using System.Collections.Generic;

public class DialogueLine
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

    public int getEmotion()
    {
        return emotion;
    }

    public override string ToString()
    {
        return "name: " + name + " content: " + content + " emotion(int): " + emotion + " position: " + position;
    }
}