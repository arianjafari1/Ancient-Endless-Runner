using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Code done by Arian- Start
[System.Serializable] public class Sound
{

    /// <summary>
    /// 27/07/2022 -[Arian] created Sound script that doesn't inherit from MonoBehaviour
    /// 27/07/2022 -[Arian] made a Sound class to use with attributes to use as array for sounds in the AudioManager
    /// </summary>

    [SerializeField] private string nameSound; //name for the property sound in the editor

    [SerializeField] private AudioClip clipSound;


    [Range(0f, 1f)] // to add a slider to those var in Unity Editor, we need to ad min and max
    [SerializeField] private float volumeSound; //volume of sound
    [Range(0f, 1f)]
    [SerializeField] private float pitchSound; //pitch of sound

    [SerializeField] private bool loopSound; //option for looping the audio

    
    private AudioSource sourceSound; //for the play method, where the audio source is stored from Audio


    //getters and setters
    public string NameSound 
    {
        get
        {
            return nameSound;
        }
        set
        {
            nameSound = value;
        }
    }

    public AudioClip ClipSound 
    {
        get
        {
            return clipSound;
        }
        set
        {
            clipSound = value;
        }
    }

    public float VolumeSound 
    {
        get
        {
            return volumeSound;
        }
        set
        {
            volumeSound = value;
        }
    }

    public float PitchSound
    {
        get
        {
            return pitchSound;
        }
        set
        {
            pitchSound = value;
        }
    }

    public bool LoopSound
    {
        get
        {
            return loopSound;
        }
        set
        {
            loopSound = value;
        }
    }

    public AudioSource SourceSound
    {
        get
        {
            return sourceSound;
        }
        set
        {
            sourceSound = value;
        }
    }
}
/// Code done by Arian- end