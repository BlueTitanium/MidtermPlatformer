using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 checkPoint;
    public string Level;
    public int currWeapon, currWeaponLength;
    public GameData()
    {
        checkPoint = new Vector3(-5.5f, -3.84f, 0);
        Level = "Level1";
        currWeapon = 0;
        currWeaponLength = 1;
    }
}
