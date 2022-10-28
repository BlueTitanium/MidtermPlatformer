using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour, IDataPersistence
{
    public string currentLevelName = "";
    public Vector3 checkPoint;
    public int weaponEquipped;
    public int weaponLength;
    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LevelManager");
        if (objects.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
                print("current level name " + currentLevelName); 
                print("current weapon " + weaponEquipped);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (currentLevelName != SceneManager.GetActiveScene().name)
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }

    public void DestroyLM()
    {
        Destroy(this.gameObject);
    }

    public void updateScene(string nextSceneName)
    {
        currentLevelName = nextSceneName;
        if(nextSceneName == "Level1")
        {
            checkPoint = new Vector3(-5.5f,-3.83999991f,0);
        }
        if(nextSceneName == "Level2")
        {
            checkPoint = new Vector3(143.5387f, -60.48101f, 0);
        }
        else if(nextSceneName == "Level3")
        {
            checkPoint = new Vector3(143.55f, -61.40804f, 0);
        }
        else if(nextSceneName == "Level4")
        {
            checkPoint = new Vector3(539.746826f,-29.5484524f,0);
        }
        print(currentLevelName);
    }

    public void ResetCheckPoint(){
        new Vector3(-5.5f, -3.83999991f, 0);
    }
    public void LoadData(GameData data)
    {
        checkPoint = data.checkPoint;
        currentLevelName = data.Level;
        weaponEquipped = data.currWeapon;
        weaponLength = data.currWeaponLength;
    }
    public void SaveData(ref GameData data)
    {
        data.checkPoint = checkPoint;
        if(currentLevelName != "MainMenu" && currentLevelName != "EndScene"){
            data.Level = currentLevelName;
        }
        if(currentLevelName == "EndScene")
        {
            data.Level = "Level1";
            data.checkPoint = new Vector3(-5.5f, -3.83999991f, 0);
        }
        data.currWeapon = weaponEquipped;
        data.currWeaponLength = weaponLength;
    }
}
