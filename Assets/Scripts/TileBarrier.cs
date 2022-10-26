using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBarrier : MonoBehaviour
{
    public GameObject tileBarrier;
    protected List<GameObject> enemiesInside = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        tileBarrier.SetActive(false);
        print("Trigger");
    }

    // Update is called once per frame
    void Update()
    {
        if(tileBarrier.activeSelf == true && enemiesInside.Count == 0)
        {
            tileBarrier.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        print("Trigger Detected " + other.gameObject.CompareTag("Player"));
        if(other.gameObject.CompareTag("Player"))
        {
            tileBarrier.SetActive(true);
        }
        else if(other.gameObject.CompareTag("Enemy") && (!enemiesInside.Contains(other.gameObject)))
        {
            print("Count " + enemiesInside.Count);
            enemiesInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(enemiesInside.Contains(other.gameObject))
        {
            enemiesInside.Remove(other.gameObject);
        }
    }
}
