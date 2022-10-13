using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public Vector3 playerPosition;
    public string Level;
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        Level = "Level1";
    }
}
