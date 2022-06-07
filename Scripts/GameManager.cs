using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

namespace paintSystem
{
    public class SaveObject
    {
        public int _currentLevel { get; set; }
        public float _musicVolume = .24f;
        public float _sfxVolume = .24f;
        public List<bool> _playerEnabledPaints { get; set; }
        public Vector3 _playerPos { get; set; }
        public ORIENTATION _playerOrientation { get; set; }
        public PAINT_TYPE _playerPaintPower { get; set; }
        public PAINT_TYPE _playerCurrentPaintAmmo { get; set; }
    }

    public class GameManager : MonoBehaviour
    {
        public int currentLevel = 0;
        //public List<bool> completedLevels = new List<bool>();
        public static GameManager managerTemplate;
        public GameObject player;

        public AudioSource sfxAudio;
        public AudioSource lvlAudio;

        public int scenes = 0;
        public int winScene = 1;
        public int lossScene = 1;
        public int completedlevels = 0;

        public string nextLevel = string.Empty;

        public float musicVolume { get; set; }
        public float sfxVolume { get; set; }

        public List<AudioClip> lvlMusicClips;

        public void UpdateVolumes()
        {
            lvlAudio.volume = musicVolume;
            sfxAudio.volume = sfxVolume;
        }

        public enum GAMESTATE
        {
            GAMEOVER,
            MENU,
            RUNNING,
            PAUSED
        }

        public GAMESTATE state = GAMESTATE.MENU;

        void OnAwake()
        {
            Load();
        }

        static string savedLevel = "SavedLevel";
        static string mscVol = "MusicVolume";
        static string sfxVol = "SFXVolume";

        public void Save()
        {
            PlayerPrefs.SetString(savedLevel, SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat(mscVol, musicVolume);
            PlayerPrefs.SetFloat(sfxVol, sfxVolume);
            PlayerPrefs.Save();
            Debug.Log("Saved.");
        }

        public void SaveVolumes()
        {
            PlayerPrefs.SetFloat(mscVol, musicVolume);
            PlayerPrefs.SetFloat(sfxVol, sfxVolume);
            PlayerPrefs.Save();
            Debug.Log("Saved Volumes.");
        }

        public void LoadVolumes()
        {
            musicVolume = PlayerPrefs.GetFloat(mscVol);
            sfxVolume = PlayerPrefs.GetFloat(sfxVol);
            Debug.Log("Loaded Volumes");
        }

        public void Load()
        {
            nextLevel = PlayerPrefs.GetString(savedLevel);
            LoadVolumes();

            if (nextLevel != string.Empty)
                SceneManager.LoadScene("LoadingScene");


        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this);

            //bool falsehood = false;

            // create a boolean for each level to indicate completion
            /*for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                completedLevels.Add(falsehood);

            scenes = completedLevels.Count;*/
            LoadVolumes();
        }

        // start functions setup a game state to run
        public void StartGameOverState(bool win)
        {
            state = GAMESTATE.GAMEOVER;

            //player.GetComponent<PlayerScript>().Kill();
            // load either the win or lose scene
            if (win)
                SceneManager.LoadScene(winScene);
            else
                SceneManager.LoadScene(lossScene);
        }

        public void StartMenuState()
        {
            state = GAMESTATE.MENU;
        }

        public void StartRunningState()
        {
            state = GAMESTATE.RUNNING;

            if (lvlAudio.isPlaying)
                lvlAudio.Stop();

            currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(currentLevel);

            Debug.Log("started game.");
        }

        public void ResumeRunningState()
        {
            state = GAMESTATE.RUNNING;
        }

        public void StartPausedState()
        {
            state = GAMESTATE.PAUSED;
        }


        // these functions are run on Update() depending on the game state
        void GameOverState()
        {
            // wait for the user to press space to return to the menu
            if (Input.GetKeyDown(KeyCode.Space))
                StartMenuState();
        }

        void MainMenuState()
        {

        }

        void PauseMenuState()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                StartPausedState();
        }

        void RunningState()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (lvlAudio.volume != musicVolume)
                lvlAudio.volume = musicVolume;

            if (sfxAudio.volume != sfxVolume)
                sfxAudio.volume = sfxVolume;

            // different updates for different states
            switch (state)
            {
                case GAMESTATE.GAMEOVER:
                    GameOverState();
                    break;
                case GAMESTATE.MENU:
                    MainMenuState();
                    break;
                case GAMESTATE.RUNNING:
                    RunningState();
                    break;
                case GAMESTATE.PAUSED:
                    PauseMenuState();
                    break;
            }

        }

        // used in the level selection screen
        public void SelectLevel(int level)
        {
            //if (completedLevels[level - 1])
            SceneManager.LoadScene(level);
        }

        public void NextLevel()
        {
            // do this before we increment, mark level complete
            //completedLevels[currentLevel] = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            if (lvlAudio.isPlaying)
                lvlAudio.Stop();

            currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("LoadingScene");
        }

        void GivePaintPower(int power)
        {
            player.GetComponent<PlayerScript>().enabledPaints[power] = true;
        }

        void RemovePaintPower(int power)
        {
            player.GetComponent<PlayerScript>().enabledPaints[power] = true;
        }

        void ClearPaintPowers()
        {
            for(int i = 0; i < player.GetComponent<PlayerScript>().enabledPaints.Count; i++)
            {
                player.GetComponent<PlayerScript>().enabledPaints[i] = false;
            }
        }

        public void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }

        public void LoadPlayer()
        {
            PlayerData data = SaveSystem.LoadPlayer();

            currentLevel = data.currentLevel;

            Vector3 playerPos;
            playerPos.x = data.playerPos[0];
            playerPos.y = data.playerPos[1];
            playerPos.z = data.playerPos[2];

            transform.position = playerPos;
        }

    }
}

