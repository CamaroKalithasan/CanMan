using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using paintSystem;
using UnityEngine.EventSystems;
using System.Xml; // for saving

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu, optionsMenu, paintHUD;
    public GameObject pausedFirstButton, optionsFirstButton, optionsCloseButton;

    public void Update()
    {
        if(!optionsMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                if (GameIsPaused)
                    Resume();
                else
                    Pause();
        } else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
            }
        }

    }

    public void Resume()
    {
        Manager.gGameManager.ResumeRunningState();
        pauseMenu.SetActive(false);
        paintHUD.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Manager.gGameManager.player.SetActive(true);
    }

   public void Pause()
   {
        Manager.gGameManager.StartPausedState();
        pauseMenu.SetActive(true);
        paintHUD.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Manager.gGameManager.player.SetActive(false);
        
   }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Save()
    {
        Manager.gGameManager.Save();
    }

    public void Load()
    {
        Manager.gGameManager.Load();
    }

    public void OpenOptions()
    {

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);

        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptions()
    {

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);

        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsCloseButton);


    }

    public void OpenPause()
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);

        //set a new selected object
        EventSystem.current.SetSelectedGameObject(pausedFirstButton);
    }
}
