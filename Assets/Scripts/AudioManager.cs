using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
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

    // Audio Sources
    public AudioSource ambiant;
    public AudioSource music;
    public AudioSource misc;
    public AudioSource mouseClick;

    // Audio Clips
    // music
    public AudioClip musicChat1;
    public AudioClip musicChat2;
    public AudioClip musicChat3;

    public AudioClip ambiantSounds;
    public AudioClip messageReceived;
    public AudioClip messageSent;
    public AudioClip click;
    public AudioClip loggingIn;
    public AudioClip loggingOff;
}
