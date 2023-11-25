using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public string character;
    public string script;
    public string emotion;
    public Image characterImage;
    public List<Choice> choices;
    
}