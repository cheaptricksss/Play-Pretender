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
    private float imageWidth = 200;

    public List<GameObject> currentChoicesOnScreen = new List<GameObject>();
    
    public List<Sequences> sequences = new List<Sequences>();
    public int sequenceIndex = 0;
    private int dialogueIndex = 0;

    public bool choiceTime = false;
    //ienumerator, waitAndPrint

    // text  components of the inbox
    public TMP_Text inboxTxt;
    public int unopenedMessages = 0;


    //knowledge check
    public int suspicionLvl = 0;

    //audio
    //public AudioClip

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
        newestGameObj = Instantiate(chatMessageObj, content.transform);

        ScrollElement(); // the refence goes in, the actual obj doesn't

        //adding an immage
        if (sequences[sequenceIndex].messages[dialogueIndex].isImage == true)
        { // should the image be only image or the text aswell?

            //Debug.Log("In Image If Statetment");
            GameObject newImObj = Instantiate(image, content.transform);
            newImObj.GetComponent<Image>().sprite = sequences[sequenceIndex].messages[dialogueIndex].image;
            //Vector2 dimensions = sequences[sequenceIndex].messages[dialogueIndex].image.
            //newImObj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 150);
            newImObj.GetComponent<Image>().rectTransform.sizeDelta =
                new Vector2(imageWidth,
                imageWidth * sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.y/ sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.x);
        }
        prevDialogueTextLength = 2;
        dialogueIndex++;

        inboxTxt.text = "Inbox";
        StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));
        charBuffer = 50;
        timeProduct = 80;

        // start playing music (dont loop)
        //start playing ambiant sound (loop)
        AudioManager.instance.ambiant.clip = AudioManager.instance.ambiantSounds;
        AudioManager.instance.ambiant.Play();
        //logging on (connected) sound

    }

    //
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
    private float charBuffer = 0;
    public GameObject newestGameObj;
    private GameObject newestButtonObj;
    public GameObject jsonsNote;
    private int prevDialogueTextLength;

    //wait and print
    public IEnumerator waitAndPrint(dialogueObj dialogue)
    {
        //float waitTime = (dialogue.text.Length) * (Time.deltaTime*timeProduct/Mathf.Log(dialogue.text.Length)); // wait this time, then restart
        float waitTime = Mathf.Log(prevDialogueTextLength + charBuffer) * Time.deltaTime * timeProduct;
        //Debug.Log(sequences[sequenceIndex].messages.Count); // shows only once
        yield return new WaitForSeconds(waitTime);
        prevDialogueTextLength = dialogue.text.Length;

        //after waiting is done
        if (dialogueIndex != sequences[sequenceIndex].messages.Count-1) //if its not the last message in the Messages list
        {
            //actually creating the text object in the screen
            newestGameObj = Instantiate(chatMessageObj, content.transform);

            ScrollElement(); // the refence goes in, the actual obj doesn't

            //adding an immage
            if(sequences[sequenceIndex].messages[dialogueIndex].isImage == true)
            { // should the image be only image or the text aswell?

                //Debug.Log("In Image If Statetment");
                GameObject newImObj = Instantiate(image, content.transform);
                newImObj.GetComponent<Image>().sprite = sequences[sequenceIndex].messages[dialogueIndex].image;

                //newImObj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 150);
                newImObj.GetComponent<Image>().rectTransform.sizeDelta =
                    new Vector2(imageWidth,
                    imageWidth * sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.y / sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.x);

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
            prevDialogueTextLength = 0;
            if (sequences[sequenceIndex].branches.Count != 0)
            {
                newestGameObj = Instantiate(chatMessageObj, content.transform);
                ScrollElement(); // the refence goes in, the actual obj doesn't
                                 //choiceTime = true;

                if (sequences[sequenceIndex].messages[dialogueIndex].isImage == true)
                { // should the image be only image or the text aswell?

                    //Debug.Log("In Image If Statetment");
                    GameObject newImObj = Instantiate(image, content.transform);
                    newImObj.GetComponent<Image>().sprite = sequences[sequenceIndex].messages[dialogueIndex].image;

                    //newImObj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 150);
                    newImObj.GetComponent<Image>().rectTransform.sizeDelta =
                        new Vector2(imageWidth,
                        imageWidth * sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.y / sequences[sequenceIndex].messages[dialogueIndex].image.bounds.size.x);

                }
                for (int i = 0; i < sequences[sequenceIndex].branches.Count; i++)// choices as buttons
                {
                    //works
                    newestButtonObj = Instantiate(choiceBox, choiceBoxContent.transform); // create and put in parent obj
                                                                                          //works
                    newestButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sequences[sequenceIndex].branches[i].choiceMsgs;
                    newestButtonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 45);
                    //change the properties of the button manually
                    //newestButtonObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta = new Vector2(450, 30);

                    //adding the obj into the series
                    currentChoicesOnScreen.Add(newestButtonObj);
                    // need a way to add the onClick function to the prefab
                    //currentChoicesOnScreen[i].GetComponent<Button>().onClick.AddListener(() => chooseDialogueOption(newestButtonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>())); // lambda expression, doesn't work
                    newestButtonObj = null;
                }
            }

            //else
            //{
            //    StartCoroutine(nextChat());
            //}

            //special event ------------------------------------ sequence specific event
            //external event sound effect
            if (sequenceIndex == 2)
            {
                currentPopUpButton = Instantiate(popUpButton, inboxButtonHolder.transform); //create pop up button
                currentPopUpButton.GetComponent<InboxButton>().attachedPopUp = Instantiate(popUp, Canvas.transform);//create pop up
                currentPopUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Note from .json";
                currentPopUpButton.GetComponent<RectTransform>().sizeDelta = new Vector2(311.647f, 80);
                //create the writing inside the pop up
                newestGameObj = Instantiate(chatMessageObj, currentPopUpButton.GetComponent<InboxButton>().attachedPopUp.GetComponent<PopUp>().contentBox.transform);
                newestGameObj.GetComponent<TextMeshProUGUI>().text = "TO: json\n" +
                    "FROM: Unknown Sender\nHello. I hope this message reaches you. I don't have much time." +
                    " You are Jason Lastname. You are an 18 year old in Acorn Falls. You are interested in " +
                    "the band 'Nuclear Love Story' and shopping at Hot Topic. There was an incident in Acorn " +
                    "Falls High School between you and your friend, Jeff.";
                int maxChar = 20;
                newestGameObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta =
                    new Vector2(186f, (30 * ((newestGameObj.GetComponent<TextMeshProUGUI>().text.Length / maxChar) + 1)));

            }
            if (sequenceIndex == 10)
            {
                currentPopUpButton = Instantiate(popUpButton, inboxButtonHolder.transform); //create pop up button
                currentPopUpButton.GetComponent<InboxButton>().attachedPopUp = Instantiate(popUp, Canvas.transform);//create pop up
                currentPopUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Note from .json";
                currentPopUpButton.GetComponent<RectTransform>().sizeDelta = new Vector2(311.647f, 80);
                //create the writing inside the pop up
                newestGameObj = Instantiate(chatMessageObj, currentPopUpButton.GetComponent<InboxButton>().attachedPopUp.GetComponent<PopUp>().contentBox.transform);
                newestGameObj.GetComponent<TextMeshProUGUI>().text = "TO: json\n" +
                    "FROM: json\nWow. you just cant seem to understand the darkness inside me..." +
                    "I thought I programmed you better. but it seems like youre just some worthless effigy, just as worthless as the real me." +
                    " I thought youd be better but guess not. i can make you better so say goodbye to everything you know." +
                    " youll be less of a disappointment this time.";
                int maxChar = 20;
                newestGameObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta =
                    new Vector2(186f, (30 * ((newestGameObj.GetComponent<TextMeshProUGUI>().text.Length / maxChar) + 1)));

            }

            sequenceIndex++;
            dialogueIndex = 0;

            if (sequences[sequenceIndex - 1].branches.Count == 0)
            {
                StartCoroutine(nextChat());
            }
        }
        else // the end of the game, no more dialogues left
        {

        }
    
}


    //public GameObject scrollBarObj;
    private void ScrollElement() //  nudging all the elements down, creating the text
    {
        // maybe have a separate list of object kept in the game manager about the sent texts
        if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.num1Wa1fuLVR) // choosing the names from the public enum
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "<color=#5C9B6E>num1Wa1fuLVR</color>";
            
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.kris)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "<color=#4E7D9C>kris</color>";
            
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.d4rknessWay)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "<color=#875590>d4rknessWay</color>";
            
        }
        else if (sequences[sequenceIndex].messages[dialogueIndex].userTag == character.json)
        {
            newestGameObj.GetComponent<TextMeshProUGUI>().text = "<color=#C23B3B>.json</color>";
            
        }
        newestGameObj.GetComponent <TextMeshProUGUI>().text += " said: " + sequences[sequenceIndex].messages[dialogueIndex].text;
        // change the  heigth and width of  the text mesh pro obj according to the amount of lines there is
        // currently thinks that theres 1 line which equals to  0
        Debug.Log(newestGameObj.GetComponent<TextMeshProUGUI>().textInfo.lineCount);
        //newestGameObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta =
        //    new Vector2(186f, (57.262f* newestGameObj.GetComponent<TextMeshProUGUI>().textInfo.lineCount));
        int maxChar = 20;
        newestGameObj.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta =
            new Vector2(186f, (30 * ((newestGameObj.GetComponent<TextMeshProUGUI>().text.Length/maxChar) + 1)));

        //message sent/received sounds

        //scrollBarObj.GetComponent<Scrollbar>().value = 0;
        //newestGameObj.GetComponent<TextMeshProUGUI>().textInfo.lineCount

        //audio
        if (sequences[sequenceIndex].messages[dialogueIndex].userTag != character.json)
        {
            AudioManager.instance.misc.clip = AudioManager.instance.messageReceived;
        }
        else
        {
            AudioManager.instance.misc.clip = AudioManager.instance.messageSent;
        }
        AudioManager.instance.misc.Play();
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

    public GameObject popUpButton;
    public GameObject popUp;
    public GameObject Canvas;
    public GameObject inboxButtonHolder;
    public GameObject currentPopUpButton;
    private int chatNumInboxTxt = 0;
    //new chat couratine
    public IEnumerator nextChat()
    {
        //write that the new chatis loading
        newestGameObj = Instantiate(chatMessageObj, content.transform);
        newestGameObj.GetComponent<TextMeshProUGUI>().text = "new chat loading";
        Debug.Log(content.transform.childCount);

        yield return new WaitForSeconds(30*Time.deltaTime); // wait for 5 seconds

        //transfer all the previous chat
        currentPopUpButton = Instantiate(popUpButton, inboxButtonHolder.transform);
        currentPopUpButton.GetComponent<InboxButton>().attachedPopUp = Instantiate(popUp, Canvas.transform);
        chatNumInboxTxt++;
        currentPopUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Chat " + chatNumInboxTxt;
        currentPopUpButton.GetComponent<RectTransform>().sizeDelta = new Vector2(311.647f, 80);
        //start the music
        if (chatNumInboxTxt == 1)
        {
            AudioManager.instance.music.clip = AudioManager.instance.musicChat2;
        }
        else
        {
            AudioManager.instance.music.clip = AudioManager.instance.musicChat3;
        }
        AudioManager.instance.music.Play();

        //delete all contents
        if (content.transform.childCount > 0)
        {
            for (int i = content.transform.childCount - 1; i >-1; i--) // pray to god it works
            {
                //it gets addedd here, that works
                content.transform.GetChild(i).gameObject.transform.parent =
                    currentPopUpButton.GetComponent<InboxButton>().attachedPopUp.GetComponent<PopUp>().contentBox.transform;
            }
        }
        //close the pop up
        currentPopUpButton.GetComponent<InboxButton>().attachedPopUp.SetActive(false);

        // you have mail!
        unopenedMessages++;
        inboxTxt.text = "Inbox (" + unopenedMessages+ ")";

        StartCoroutine(waitAndPrint(sequences[sequenceIndex].messages[dialogueIndex]));

        //start playing audio according to the chat
    }

    public void closeSelectedPopUp(GameObject popUp)
    {
        popUp.SetActive(false);
    }
}
