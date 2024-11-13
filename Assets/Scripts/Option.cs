using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Option
{
    public string content;
    public string next;    
    public List<string> flags;

    //new
    public int powerReq0;
    public int powerReq1;
    public int powerReq2;

    public Option (string content)
    {
        this.content = content;
    }

    public Option(string content, string next)
    {
        this.content = content;
        this.next = next;
    }

    public Option(string content, string next, string req)
    {
        this.content = content;
        this.next = next;
        string[] reqs = req.Split("-");
        if (reqs[0] == "0")
        {
            powerReq0 = int.Parse(reqs[1].ToString());
        } else if (reqs[0] == "1")
        {
            powerReq1 = int.Parse(reqs[1].ToString());
        } else if (reqs[0] == "2")
        {
            powerReq2 = int.Parse(reqs[1].ToString());
        }
    }

    public override string ToString()
    {
        return $"Content:{content}, Next:{next}";
    }
}