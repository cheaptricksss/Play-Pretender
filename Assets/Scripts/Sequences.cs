using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequences 
{
    public List<dialogueObj> messages = new List<dialogueObj>();
    public List<Choices> branches = new List<Choices>();
}
