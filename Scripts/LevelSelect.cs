using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MenuInterface
{

    public GameObject levelLoadButton;
    public GameObject scene;
    public List<string> levels = new List<string>();

    void CreateLevelButton(string sceneName)
    {
       
    }

    void CreateLevelList()
    {
        foreach(string level in levels)
        {
            CreateLevelButton(level);
        }
    }

    public void ReLoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        ClickNoise();
        SceneManager.LoadScene(scene.name);
    }
}
