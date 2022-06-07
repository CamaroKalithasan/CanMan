using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using paintSystem;

public class MenuInterface : MonoBehaviour
{
    // AUDIO
    public AudioSource audioPlayer;
    //public AudioSource musicPlayer; // used for old background music system
    public AudioClip sButtonClick;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void PlayOnce()
    {
        if (!audioPlayer)
            Debug.Log("No audio player!");

        if (audioPlayer.isPlaying)
            audioPlayer.Stop();

        // no spatial audio in menu, play the sound once
        audioPlayer.loop = false;
        audioPlayer.Play();
        Debug.Log("Played Audio.");
    }

    public void ClickNoise()
    {
        audioPlayer.clip = sButtonClick;
        PlayOnce();
    }
}
