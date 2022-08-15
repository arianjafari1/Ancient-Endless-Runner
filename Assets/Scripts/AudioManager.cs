using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// Code done by Arian- Start

    /// <summary>
    /// 27/07/2022 -[Arian] created AudioManager script
    ///            -[Arian] using the sounds class to created a Serialized array to add audio in the AudioManager object in the editor
    ///            -[Arian] Made sure that in Awake there is no two or more instances of the audio manager with a check
    ///            -[Arian] Added function to play sound
    /// 03/08/2022 -[Arian] Added a stop function to stop sound already playing
    /// </summary>

    [SerializeField] private Sound[] sounds;
    private static AudioManager audioManagerInstance; //static reference to the current audio manager that we have on our scene


    private void Awake() //similar to start method but called right before it
    {
        if (audioManagerInstance == null) // if we don't have an audio manager in our scene
        {
            audioManagerInstance = this; // then instance equals to this object
        }
        else
        {
            Destroy(gameObject); // destroy the other audio managers to avoid duplicates
            return; //just to be sure that no more coded will be executed after destroying the object
        }




        foreach (Sound sound in sounds) //looping through a list of sounds and add an audio source for each
        {
            //add the audio source component and store in a var, so that when we want to play a sound, we can call the play method on the source
            sound.SourceSound = gameObject.AddComponent<AudioSource>();
            //with these any modifications we make from the Unity Editor will actually take effect
            sound.SourceSound.clip = sound.ClipSound; //clip of our audio source
            sound.SourceSound.volume = sound.VolumeSound;
            sound.SourceSound.pitch = sound.PitchSound;
            sound.SourceSound.loop = sound.LoopSound; //assigns the loop option from the audio manager to the loopSound from Sound class, same for the other ones
        }


    }


    private void playSound(string name) //method to play sound
    {
        //loop through all the audio and find the one with the correct name
        Sound sound = Array.Find(sounds, sound => sound.NameSound == name); //find the sound in sounds where sound name is equal to name, and store it to var sound

        if (sound == null) //check if the sound is null, if the name exists to begin with, maybe if you spell it wrong for example
        {
            return; //returning nothing
        }

        sound.SourceSound.Play();
    }

    public void PlaySound(string Name)
    {
        playSound(Name);
    }


    public void StopSound(int audioSourceNumberInArray) //function to stop sound
    {
        for (int i = 0; i < sounds.Length; i++) //looping through a list of sounds to look for audio sources while the game is playing
        {
            try //ignore all errors
            {
                AudioSource soundToStop = sounds[audioSourceNumberInArray].SourceSound; // the audio source that you want, based on parameter 
                                                                                    //that you input, will be added to this AudioSource variable
                soundToStop.Stop(); //take the AudioSource variable and stop the sound
            }
            catch { }


            
        }
    }



    /// Code done by Arian- end
}
