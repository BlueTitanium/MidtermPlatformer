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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevelName != SceneManager.GetActiveScene().name)
        {
            Destroy(this.gameObject);
        }
    }

    public void DestroyLM()
    {
        Destroy(this.gameObject);
    }
}
