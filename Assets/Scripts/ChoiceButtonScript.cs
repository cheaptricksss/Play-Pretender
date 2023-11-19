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
        // mouse clicking sound
        AudioManager.instance.mouseClick.Play();

        //Debug.Log("Button Has Been Clicked");
        //Debug.Log("TMPtext is: " + txt.text);
        //GameManager.instance.choiceTime = true;
        //if the chosen dialogue has flavor text, add the flavor text inside the next Sequence texts (to the start)
        for (int i = 0; i < GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches.Count; i++)
        {
            if (GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].choiceMsgs == txt.text) //find the one with the matching text
            {
                if (GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].isKnowladgeCheck == true &&
                    GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].suspicion == Choices.knowladgeCheck.suspicious)
                {
                    GameManager.instance.suspicionLvl++;  
                }

                if (GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count != 0)
                {
                    //Debug.Log("Branch had more than 0 elements");
                    //Debug.Log(GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count);
                    for (int l = GameManager.instance.sequences[GameManager.instance.sequenceIndex - 1].branches[i].flavorDialogues.Count - 1; l > -1; l--)
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
        Debug.Log("Suspicion Lvl is: "+GameManager.instance.suspicionLvl);


        if (GameManager.instance.sequenceIndex == 9 && GameManager.instance.suspicionLvl < 1) // the branch point
        {
            GameManager.instance.currentPopUpButton = Instantiate(GameManager.instance.popUpButton, GameManager.instance.inboxButtonHolder.transform); //create pop up button
            GameManager.instance.currentPopUpButton.GetComponent<InboxButton>().attachedPopUp = Instantiate(GameManager.instance.popUp, GameManager.instance.Canvas.transform);//create pop up
            GameManager.instance.currentPopUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Note from .json";
            GameManager.instance.currentPopUpButton.GetComponent<RectTransform>().sizeDelta = new Vector2(311.647f, 80);
            //create the writing inside the pop up
            GameManager.instance.newestGameObj = Instantiate(GameManager.instance.chatMessageObj, GameManager.instance.currentPopUpButton.GetComponent<InboxButton>().attachedPopUp.GetComponent<PopUp>().contentBox.transform);
            GameManager.instance.newestGameObj.GetComponent<TextMeshProUGUI>().text = "TO: json\n" +
                "FROM: json\nHeh... looks like you've done a lot better than all the other ones." +
                " i knew soon enough my program would capture the real abyss of my heart. its beautiful, in a dark way... " +
                "nobody could ever understand me. It took a cold, soulless machine to do it instead. but enough talk." +
                " ill let this version keep running, since it seems safe for now. good work, 'Jason'...";
        }
        else  // the branch point
        {
            GameManager.instance.StartCoroutine(GameManager.instance.waitAndPrint(GameManager.instance.sequences[GameManager.instance.sequenceIndex].messages[0]));
        }
    }
}
