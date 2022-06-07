using paintSystem;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   public static void SavePlayer(GameManager player)
   {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(fileStream, data);
        fileStream.Close();

        Debug.LogError("File saved.");
   }

    public static PlayerData LoadPlayer()
    {

        if(File.Exists(Application.persistentDataPath + "/Player.dat"))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Open);

            PlayerData data = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            Debug.LogError("Save file loaded.");

            return data;
        }

        else
        {
            Debug.LogError("Save file not found in " + Application.persistentDataPath + "/Player.dat");
            return null;
        }
    }
}
