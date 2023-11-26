using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Option
{
    public string content;
    public string next;
    public List<string> flags;

    public Option(string content, string next)
    {
        this.content = content;
        this.next = next;
    }

    public override string ToString()
    {
        return $"Content:{content}, Next:{next}";
    }
}