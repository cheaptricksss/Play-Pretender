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

    public List<GameObject> currentChoicesOnScreen = new List<GameObject>();
    
    public List<Sequences> sequences = new List<Sequences>();
    public int sequenceIndex = 0;
    private int dialogueIndex = 0;

    public bool choiceTime = false;
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
        if (choiceTime == true)
        {
            Debug.Log("is in the if statement");
            ClearChoiceList();
            choiceTime = false;
        }
    }

    //ienumerator, waitAndPrint variables
    private int charPerSec = 5;
    public float timeProduct = 1;
    public GameObject newestGameObj;
    private GameObject newestButtonObj;
    public GameObject jsonsNote;

    //wait and print
    public IEnumerator waitAndPrint(dialogueObj dialogue)
    {
        //float waitTime = (dialogue.text.Length) * (Time.deltaTime*timeProduct/Mathf.Log(dialogue.text.Length)); // wait this time, then restart
        float waitTime = Mathf.Log(dialogue.text.Length) * Time.deltaTime * timeProduct;
        //Debug.Log(sequences[sequenceIndex].messages.Count); // shows only once
        yield return new WaitForSeconds(waitTime);

        //after waiting is done
        if (dialogueIndex != sequences[sequenceIndex].messages.Count-1)
        {
            //actually creating the text object in the screen
            newestGameObj = Instantiate(chatMessageObj, content.transform);

            ScrollElement(); // the refence goes in, the actual obj doesn't
            if(sequences[sequenceIndex].messages[dialogueIndex].isImage == true)

            { // should the image be only image or the text aswell?

                //Debug.Log("In Image If Statetment");
                GameObject newImObj = Instantiate(image, content.transform);
                newImObj.GetComponent<Image>().sprite = sequences[dialogueIndex].messages[dialogueIndex].image;
                //newImObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta = new Vector2(100, 100);
            }
            //GameObject newObj = Instantiate(chatMessageObj);
            //ScrollElement(newObj);

            Debug.Log(dialogueIndex);
            //before the player has a choice, the loop just continues
            dialogueIndex++;
            //if(dialogueIndex != sequences[sequenceIndex].messages.Count - 2)
            StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));
        }
        else if (sequenceIndex != sequences.Count - 1)// a choice needs to be made, dialogue index is at the end
        {
            newestGameObj = Instantiate(chatMessageObj, content.transform);
            ScrollElement(); // the refence goes in, the actual obj doesn't
            //choiceTime = true;
            for (int i = 0; i < sequences[sequenceIndex].branches.Count; i ++)// choices as buttons
            {
                //works
                newestButtonObj = Instantiate(choiceBox, choiceBoxContent.transform); // create and put in parent obj
                //works
                newestButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sequences[sequenceIndex].branches[i].choiceMsgs; //theres an error here
                //adding the obj into the series
                currentChoicesOnScreen.Add(newestButtonObj);
                // need a way to add the onClick function to the prefab
                //currentChoicesOnScreen[i].GetComponent<Button>().onClick.AddListener(() => chooseDialogueOption(newestButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>())); // lambda expression, doesn't work
                newestButtonObj = null;
            }

            //special event ------------------------------------ sequence specific event
            if (sequenceIndex == 2)
            {
                jsonsNote.SetActive(true);
            }

            sequenceIndex++;
            dialogueIndex = 0;
        }
        else // the end of the game, no more dialogues left
        {

        }
        //}// while true's  squigly line
    }

    private void ScrollElement() //  nudging all the elements down, creating the text
    {
        // maybe have a separate list of object kept in the game manager about the sent texts
        if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.num1Wa1fuLVR) // choosing the names from the public enum
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "num1Wa1fuLVR";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.kris)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "kris";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.d4rknessWay)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "d4rknessWay";
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.json)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = ".json";
        }
        newestGameObj.GetComponent <TextMeshProUGUI>().text += " said " + sequences[sequenceIndex].messages[dialogueIndex].text;
    }

    //public void chooseDialogueOption(TMP_Text txt) //when the player makes (clicks the button) a choice
    //{
    //    Debug.Log("Button Has Been Clicked");
    //    Debug.Log("TMPtext is: " + txt.text);
    //    choiceTime = false;
    //    //if the chosen dialogue has flavor text, add the flavor text inside the next Sequence texts (to the start)
    //    for (int i = 0; i < sequences[sequenceIndex - 1].branches.Count; i++)
    //    {
    //        if (sequences[sequenceIndex - 1].branches[i].choiceMsgs == txt.text)
    //        {
    //            //Debug.Log("Branch had more than 0 elements");
    //            if (sequences[sequenceIndex - 1].branches[i].flavorDialogues.Count != 0)
    //            {
    //                Debug.Log("Branch had more than 0 elements");
    //                Debug.Log(sequences[sequenceIndex - 1].branches[i].flavorDialogues.Count);
    //                for (int l = 0; l < sequences[sequenceIndex - 1].branches[i].flavorDialogues.Count; l++)
    //                {
    //                    sequences[sequenceIndex].messages.Insert(0, sequences[sequenceIndex - 1].branches[i].flavorDialogues[i]);
    //                }
    //            }
    //            else Debug.Log("not more than 0"); 
    //            break;
    //        }
    //    }
    //    //delete all the choice boxes at the end
    //    for (int i = 0; i < currentChoicesOnScreen.Count; i++)
    //    {
    //        Debug.Log(currentChoicesOnScreen.Count);
    //        Destroy(currentChoicesOnScreen[0]); // delete the obj on screen
    //        currentChoicesOnScreen.RemoveAt(0); // delete the reference to that obj
    //    }

    //    //while (currentChoicesOnScreen.Count != 0)
    //    //{
    //    //    Destroy(currentChoicesOnScreen[0]); // delete the obj on screen
    //    //    currentChoicesOnScreen.RemoveAt(0); // delete the reference to that obj
    //    //}
    //    currentChoicesOnScreen.TrimExcess();
    //    //start the couratine
    //    StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));
    //}

    public void ClearChoiceList()
    {
        for (int i = 0; i < currentChoicesOnScreen.Count; i++)
        {
            Debug.Log(currentChoicesOnScreen.Count);
            Destroy(currentChoicesOnScreen[i]); // delete the obj on screen
            //currentChoicesOnScreen.RemoveAt(i); // delete the reference to that obj
        }
        currentChoicesOnScreen.Clear();
        currentChoicesOnScreen.TrimExcess();
        Debug.Log("choices left in the list"+currentChoicesOnScreen.Count);
    }
}
