using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// unused, was for puzzles
/*public class ToggleableOject : MonoBehaviour
{
    public bool locked = true;
}*/

namespace paintSystem
{
    public class DoorBehavior : MonoBehaviour
    {
        // Variable Declarations:
        // Variable that stores a new sprite to switch to, chosen in the door's inspector. 
        public Sprite newSprite;
        // Variable that stores the name of the next level as a string, inputted in the door's inspector. 
        public string nextLevel;
        // Variable that stores the level completed screen as a game object, set active when the player reaches the door
        //public GameObject LevelCompletedScreen;

        public bool GameWin = false;

        public AudioSource winSource;
        public AudioClip winClip;

        // Function definitions:
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                GetComponent<SpriteRenderer>().sprite = newSprite;

                Manager.gGameManager.nextLevel = nextLevel;
                winSource.clip = winClip;
                winSource.Play();

                if (!GameWin)
                    SceneManager.LoadScene("LoadingScene");
                else
                    SceneManager.LoadScene("WinScene");
            }
        }
    }
}

