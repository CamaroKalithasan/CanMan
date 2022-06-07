using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using paintSystem;

public class AutoVolume : MonoBehaviour
{

    public AudioSource source;
    public bool SFX;

    void SetVolume()
    {
        float vol = 0.0f;

        if (SFX)
            vol = Manager.gGameManager.sfxVolume;
        else
            vol = Manager.gGameManager.musicVolume;

        source.volume = vol;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (SFX)
        {
            if (source.volume != Manager.gGameManager.sfxVolume)
                SetVolume();
        }
        else 
            if (source.volume != Manager.gGameManager.musicVolume)
                SetVolume();

    }
}
