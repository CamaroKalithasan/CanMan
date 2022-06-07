using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using paintSystem;

public class LevelButton : MonoBehaviour
{
    public int sceneIndex;

    // Method for loading scene
    public void LoadScene()
    {
        //ClickNoise(); // do in editor
        /*if (sceneIndex - 1 > Manager.gGameManager.completedlevels)
        {
            Debug.Log("Level not available.");
            return;
        }*/

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadPauseMenu()
    {
        //ClickNoise();
        Debug.Log("Load Pause Menu");
    }

    public void PassButton()
    {

    }

    void Start()
    {
        if (sceneIndex > Manager.gGameManager.completedlevels)
            Debug.Log("not available");
    }

    // Main Menu

    // Graphics Settings

    //Button checks to see if index is cleared

    // If index is complete, turn it on, else, turn it off
}
