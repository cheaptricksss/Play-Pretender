using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choices 
{
    public string choiceMsgs;
    public List<dialogueObj> flavorDialogues = new List<dialogueObj>();
    public bool isKnowladgeCheck;
    public enum knowladgeCheck {suspicious, notSuspicious}
    public knowladgeCheck suspicion;
    //dialogueObj dialogue;
}
