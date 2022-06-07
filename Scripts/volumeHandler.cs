using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using paintSystem;

public class volumeHandler : MonoBehaviour
{

    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject slidersMenu;

    public void SaveVolumes()
    {   
        // the sliders need their parent component to be active
        // in order for us to make changes
        if (!slidersMenu.activeSelf)
            return;

        Manager.gGameManager.SaveVolumes();
    }

    public void UpdateVolumes()
    {
        if (!slidersMenu.activeSelf)
            return;

        Manager.gGameManager.musicVolume = musicSlider.value;
        Manager.gGameManager.sfxVolume = sfxSlider.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!slidersMenu.activeSelf)
            return; 

        musicSlider.value = Manager.gGameManager.musicVolume;
        sfxSlider.value = Manager.gGameManager.sfxVolume;
    }
}
