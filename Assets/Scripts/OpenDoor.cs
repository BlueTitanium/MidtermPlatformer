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

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("true");
            FindObjectOfType<LevelManager>().DestroyLM();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

