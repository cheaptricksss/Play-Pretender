using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum character { num1Wa1fuLVR, kris, d4rknessWay, json } // creating the enum

[CreateAssetMenu(fileName = "NewDialogue", menuName = "DialogueObjs")]
[System.Serializable]
public class dialogueObj //: ScriptableObject
{
    //[System.Serializable]
    //public enum character {num1Wa1fuLVR, kris, d4rknessWay, json} // creating the enum
    public character userTag; // accessing fromthe inspector
    public string text;
    public bool isImage = false;

    //public TMP_Text txtBox;
    public Sprite image;
}
