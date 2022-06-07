using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using paintSystem;

public class SaveManager : MonoBehaviour
{
    //I'm not sure if I'm using this, but I'm keeping it just in case. I'll delete it if necessary
    //once the Alpha period is up on Wednesday.

    public bool hasLoaded;
    public SerializableVector3 playerPos;

    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        //Helper method inside the struct of getting the position of the player
        public Vector3 GetPlayerPos()
        {
            return new Vector3(x, y, z);
        }

        public SerializableVector3 GetPlayerPosSerializable()
        {
            return new SerializableVector3(x, y, z);
        }
    }
        //A way of creating a single object that is accessible from every other script within the project
        private void Awake()
        {
            //If there's any information, it'll save here
            Save();
        }
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DeleteSaveData();
        }
    }

    //Saves the game
    public void Save()
    {
        
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerScript.dat", FileMode.OpenOrCreate);
        //Binary formatter
        BinaryFormatter formatter = new BinaryFormatter();
        foreach(var x in Manager.gGameManager.player.GetComponent<PlayerMovement>().enabledPaints)
        {
            formatter.Serialize(fileStream, x);//Serialization method
        }

        fileStream.Close();

        Debug.Log("File saved.");

        //casting paint to a file

        //PAINT_TYPE PAINT = PAINT_TYPE.STICK;


        //int use = (int)PAINT;
        //PAINT = (PAINT_TYPE)use;
    }


    //Loads the game, functions similarly to save
    public void Load()
    {

        if(System.IO.File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Open);
            //Binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Deserialize(fileStream);
            fileStream.Close();

            Debug.Log("Save file loaded.");

            hasLoaded = true;
        }
    }

    public void DeleteSaveData()
    {

        if (System.IO.File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            File.Delete(Application.persistentDataPath + "/Player.dat");
        }

        Debug.Log("Save file deleted.");
    }
}


[System.Serializable]
public class SaveData : SaveManager
{
    public List<bool> paints;
}
