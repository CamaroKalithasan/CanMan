using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using paintSystem;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public float[] playerPos;

    public PlayerData(GameManager player)
    {
        currentLevel = player.currentLevel;

        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;
    }
}
