using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
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
            print("Position before" + GameObject.FindObjectOfType<LevelManager>().checkPoint);
            GameObject.FindObjectOfType<LevelManager>().checkPoint = transform.position;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            print("Position after " + GameObject.FindObjectOfType<LevelManager>().checkPoint);
        }
    }
}
