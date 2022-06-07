using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using paintSystem;
using UnityEngine.EventSystems;

public class MainMenu : MenuInterface
{
    public GameObject playMenu, optionsMenu, playMenuFirstButton, optionsFirstButton, optionsClosedButton;
    public void Play()
    {
        //ClickNoise(); // play click noise
        Manager.gGameManager.StartRunningState(); // run the game
    }

    public void LevelSeclect()
    {
        //ClickNoise(); // do in editor
        SceneManager.LoadScene("LevelSelectScreen");
    }

    public void Menu()
    {
        if (!playMenu.activeInHierarchy)
        {
            playMenu.SetActive(true);

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new selected object
            EventSystem.current.SetSelectedGameObject(playMenuFirstButton);
        }

        else
        {
            playMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }
    }

    public void Quit()
    {
        ClickNoise();
        Debug.Log("Quit the game");
        Application.Quit();
    }

    public void OpenOptions()
    {
        playMenu.SetActive(false);
        optionsMenu.SetActive(true);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptions()
    {
        playMenu.SetActive(true);
        optionsMenu.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
        //SceneManager.LoadScene(0);

    }
}
