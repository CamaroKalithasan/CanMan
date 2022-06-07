using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace paintSystem
{
    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(SFXMaker))]
    public class Manager : MonoBehaviour
    {
        private static GameManager gameManager;
        private static SFXMaker sfxManager;
        public static GameManager gGameManager
        {
            get
            {
                return gameManager;
            }
        }

        public static SFXMaker gSFXmanager
        {
            get
            {
                return sfxManager;
            }
        }

        void Awake()
        {
            gameManager = GetComponent<GameManager>();
            sfxManager = GetComponent<SFXMaker>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}