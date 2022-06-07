using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using paintSystem;

public class Loading : MonoBehaviour
{
    public bool waitForInput = false;

    public GameObject LoadingTxtObj;
    public GameObject ContinueTxtObj;
    public GameObject persistentScreen;

    AsyncOperation loadingOp;

    public void Awake()
    {
        if (waitForInput)
            DontDestroyOnLoad(persistentScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadLevel());
    }

    IEnumerator loadLevel()
    {
        //start loading the scene
        loadingOp = SceneManager.LoadSceneAsync(Manager.gGameManager.nextLevel);

        while(!loadingOp.isDone)
            yield return null;

        if(!waitForInput)
            persistentScreen.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForInput)
        {
            Time.timeScale = 0;
            LoadingTxtObj.SetActive(false);
            ContinueTxtObj.SetActive(true);

            // merc the loading screen
            if (Input.GetKeyDown(KeyCode.Space))
            {
                persistentScreen.SetActive(false);
                Time.timeScale = 1;
            }

            return;
        }
    }
}
