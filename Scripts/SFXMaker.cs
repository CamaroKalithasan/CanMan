using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using paintSystem;

public class SFXMaker : MonoBehaviour
{
    public AudioSource source;

    public AudioClip impact;
    public AudioClip death;
    public AudioClip walking;
    public AudioClip paintWalking;
    public AudioClip paintCreate;
    public AudioClip paintDestory;
    public AudioClip paintCanPickup;
    public AudioClip levelComplete;
    public AudioClip doorOpen;
    public AudioClip shockCharged;
    public AudioClip shock;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // only use the sfx volume for these
        if (source.volume != Manager.gGameManager.sfxVolume)
            source.volume = Manager.gGameManager.sfxVolume;
    }

    public void PlayOnce(AudioClip _efct)
    {
        source.clip = _efct;
        source.Play();

        Debug.Log("Played clip: " + _efct);
    }

    public void PlayLoop(AudioClip _efct)
    {
        source.clip = _efct;
        source.loop = true;
        source.Play();
    }

    public void StopLoop()
    {
        source.loop = false;
        source.Stop();
    }

    public void ImpactSound()
    {
        PlayOnce(impact);
    }

    public void DeathSound()
    {
        PlayOnce(death);
    }

    public void WalkingSoundLoop()
    {
        PlayLoop(walking);
    }

    public void WalkSound()
    {
        PlayOnce(walking);
        Debug.Log("Walking Sound");
    }

    public void PaintWalkingSoundLoop()
    {
        PlayLoop(paintWalking);
    }

    public void PaintWalkingSound()
    {
        PlayOnce(paintWalking);
    }

    public void PaintCreateLoop()
    {
        PlayLoop(paintCreate);
    }

    public void PaintDestroy()
    {
        PlayOnce(paintDestory);
    }

    public void PaintCanPickup()
    {
        PlayOnce(paintCanPickup);
    }

    public void LevelComplete()
    {
        PlayOnce(levelComplete);
    }

    public void DoorOpen()
    {
        PlayOnce(doorOpen);
    }

    public void ShockCharged()
    {
        PlayOnce(shockCharged);
    }

    public void Shock()
    {
        PlayOnce(shock);
    }
}
