using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public string currentLevelName = "";
    public Vector3 checkPoint;
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
        if(nextSceneName == "Level2")
        {
            checkPoint = new Vector3(143.5387f, -60.48101f, 0);
        }
        print(currentLevelName);
    }
}
