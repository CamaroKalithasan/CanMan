using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using paintSystem;

public class triggers : MonoBehaviour
{

    //add to this to create new triggers
    public AudioSource GameOver;
    public enum TRIGGER_ACTION
    {
        KILL = 0,
        GAME_WIN,
    }

    public TRIGGER_ACTION action;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (action)
        {
            case TRIGGER_ACTION.KILL:
                KillPlayer();
                break;
            case TRIGGER_ACTION.GAME_WIN:
                GameWin();
                break;
        }
    }

    // kill the player on entry
    void KillPlayer()
    {   
        GameOver.Play();
        Manager.gGameManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("LossScene");
    }

    void GameWin()
    {
        SceneManager.LoadScene("WinScene");
    }
}
