using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ChoiceButtonScript : MonoBehaviour
{
    public TMP_Text txt;
    public void chooseDialogueOption() //when the player makes (clicks the button) a choice
    {
        //Debug.Log("Button Has Been Clicked");
        Debug.Log("TMPtext is: " + txt.text);
        //GameManager.instance.choiceTime = true;
        //if the chosen dialogue has flavor text, add the flavor text inside the next Sequence texts (to the start)
        for (int i = 0; i < GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches.Count; i++)
        {
            if (GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].choiceMsgs == txt.text) //find the one with the matching text
            {

                if (GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count != 0)
                {
                    //Debug.Log("Branch had more than 0 elements");
                    //Debug.Log(GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count);
                    for (int l = 0; l < GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count; l++)
                    {
                        Debug.Log(GameManager.instance.sequenceIndex);
                        GameManager.instance.sequences[GameManager.instance.sequenceIndex].messages.Insert(0, GameManager.instance.sequences[(GameManager.instance.sequenceIndex) - 1].branches[i].flavorDialogues[l]);
                    }
                }
                //create the
                //GameManager.instance.newestGameObj = Instantiate(GameManager.instance.chatMessageObj, GameManager.instance.content.transform);
                //GameManager.instance.newestGameObj.GetComponent<TextMeshProUGUI>().text = "<color=#C23B3B>.json</color> said " + txt.text;

                break;// end the loop after finding the item
            }
        }
        //delete all the choice boxes at the end
        //GameManager.instance.ClearChoiceList(); // call clearchoice  list
        GameManager.instance.choiceTime = true;
        //start the couratine
        GameManager.instance.StartCoroutine(GameManager.instance.waitAndPrint(GameManager.instance.sequences[GameManager.instance.sequenceIndex].messages[0]));
    }
}
