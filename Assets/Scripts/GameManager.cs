using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public List<string> dialogue = new List<string>(); // the dialogue list
    //public static Dictionary<string, Dictionary<string, string>> dialogue = new Dictionary<string, Dictionary<string, string>>();
    //[SerializeField]
    //private List<dialogueObj> dialogues = new List<dialogueObj>();
    //public ScriptableObject dialogue;
    public List<Sequences> sequences = new List<Sequences>();
    

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
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void IEnu
}
