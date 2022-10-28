using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OpenDoor : MonoBehaviour
{
    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<LevelManager>().updateScene(nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }*/
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<LevelManager>().updateScene(nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

