using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject chatMessageObj; // dialogue obj
    public GameObject choiceBox; // choices (button objs)
    public GameObject choiceBoxContent; // the choice obj parent
    public GameObject content; // dialogue obj parent
    public GameObject image; // image prefab 

    private List<GameObject> currentChoicesOnScreen = new List<GameObject>();
    
    public List<Sequences> sequences = new List<Sequences>();
    private int sequenceIndex = 0;
    private int dialogueIndex = 0;

    private bool choiceTime = false;
    //ienumerator, waitAndPrint

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // delete all the placeholder objects under contents
        if (content.transform.childCount > 0)
        {
            for (int i = 0; i < content.transform.childCount; i++) // pray to god it works
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
        }
        // delete all the placeholder objects under contents
        if(choiceBoxContent.transform.childCount > 0)
        {
            for (int i = 0; i < choiceBoxContent.transform.childCount; i++) // pray to god it works
            {
                Destroy(choiceBoxContent.transform.GetChild(i).gameObject);
            }
        }
    }

    void Start() //start printing the messages as soon as the game starts 
    {
        //start couratine
        StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));
    }

    // Update is called once per frame
    void Update()
    {
        //if (choiceTime == true)
        //{


        //}
    }

    //ienumerator, waitAndPrint variables
    float timeProduct = 1;

    //wait and print
    private IEnumerator waitAndPrint(dialogueObj dialogue)
    {
        float waitTime = dialogue.text.Length * Time.deltaTime;

        while (true)
        {
            // maybe add a line that stops the corrotine from happening when the player changes the page (?)
            yield return new WaitForSeconds(waitTime);
            //after waiting is done
            if (dialogueIndex != sequences[sequenceIndex].messages.Count - 1)
            {
                //actually creating the text object in the screen
                
                    GameObject newObj = Instantiate(chatMessageObj, content.transform);
                    ScrollElement(newObj); // the refence goes in, the actual obj doesn't
               
                if(sequences[sequenceIndex].messages[dialogueIndex].isImage == true)
                { // should the image be only image or the text aswell?
                    GameObject newImObj = Instantiate(image, content.transform);
                    newImObj.GetComponent<UnityEngine.UI.Image>().sprite = sequences[dialogueIndex].messages[dialogueIndex].image;
                }
                //GameObject newObj = Instantiate(chatMessageObj);
                //ScrollElement(newObj);


                //before the player has a choice, the loop just continues
                dialogueIndex++;
                waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]);
            }
            else if (sequenceIndex != sequences.Count - 1)// a choice needs to be made, dialogue index is at the end
            {
                sequenceIndex++;
                dialogueIndex = 0;
                choiceTime = true;
                for (int i = 0; i < sequences[sequenceIndex].branches.Count; i ++)
                {
                    GameObject newObj = Instantiate(choiceBox, choiceBoxContent.transform); // create and put in parent obj
                    // add the text
                    newObj.GetComponent<UnityEngine.UI.Text>().text = sequences[sequenceIndex].branches[i].choiceMsgs;
                    currentChoicesOnScreen.Add(newObj);
                    //newObj.transform.TMP
                }
            }
            else // the end of the game, no more dialogues left
            {

            }
        }
    }

    private void ScrollElement(GameObject newObj) //  nudging all the elements down, creating the text
    {
        // maybe have a separate list of object kept in the game manager about the sent texts
        if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.num1Wa1fuLVR) // choosing the names from the public enum
        {
            newObj.GetComponent<UnityEngine.UI.Text>().text = "num1Wa1fuLVR";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.kris)
        {
            newObj.GetComponent<UnityEngine.UI.Text>().text = "kris";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.d4rknessWay)
        {
            newObj.GetComponent<UnityEngine.UI.Text>().text = "d4rknessWay";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.json)
        {
            newObj.GetComponent<UnityEngine.UI.Text>().text = ".json";
        }
        newObj.GetComponent < UnityEngine.UI.Text >().text = " said " + sequences[sequenceIndex].messages[dialogueIndex].text;
    }

    public void chooseDialogueOption(TMP_Text txt) //when the player makes (clicks the button) a choice
    {
        choiceTime = false;
        //if the chosen dialogue has flavor text, add the flavor text inside the next Sequence texts (to the start)
        for (int i = 0; i < sequences[sequenceIndex - 1].branches.Count; i++)
        {
            if (sequences[sequenceIndex - 1].branches[i].choiceMsgs == txt.text)
            {
                if (sequences[sequenceIndex - 1].branches[i].flavorDialogues.Count != 0)
                {
                    for (int l = 0; l < sequences[sequenceIndex - 1].branches[i].flavorDialogues.Count; l++)
                    {
                        sequences[sequenceIndex].messages.Insert(0, sequences[sequenceIndex - 1].branches[i].flavorDialogues[i]);
                    }
                }
                break;
            }
        }
        //delete all the choice boxes at the end
        while (currentChoicesOnScreen.Count != 0)
        {
            Destroy(currentChoicesOnScreen[0]); // delete the obj on screen
            currentChoicesOnScreen.RemoveAt(0); // delete the reference to that obj
        }
        currentChoicesOnScreen.TrimExcess();
        //start the couratine
        StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));
    }
}
