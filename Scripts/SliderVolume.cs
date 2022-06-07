using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using paintSystem;

public class SliderVolume : MonoBehaviour
{
    public Slider slider;
    public bool SFX;

    // Start is called before the first frame update
    void Start()
    {
        float vol = Manager.gGameManager.musicVolume;

        if (SFX)
            vol = Manager.gGameManager.sfxVolume;

        slider.value = vol;
    }
}
