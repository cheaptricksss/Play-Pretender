using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InboxButton : MonoBehaviour
{
    public bool newMail = true;
    public GameObject attachedPopUp;

    public void OpenPopUp()
    {
        if (newMail == true)
        {
            //change the text on the inbox tag
            newMail = false;
            GameManager.instance.unopenedMessages--;
            if (GameManager.instance.unopenedMessages > 0)
            {
                GameManager.instance.inboxTxt.text = "Inbox (" + GameManager.instance.unopenedMessages + ")";

            }
            else
            {
                GameManager.instance.inboxTxt.text = "Inbox";
            }
        }

        //activate the object
        attachedPopUp.SetActive(true);
    }

    public void ClosePopUp()
    {
        //deactivate the object
        GameManager.instance.closeSelectedPopUp(attachedPopUp);
    }
}
