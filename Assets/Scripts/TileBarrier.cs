using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBarrier : MonoBehaviour
{
    public GameObject tileBarrier;
    public GameObject spawnBoss;
    public List<GameObject> enemiesInside = new List<GameObject>();
    public GameObject bossCam;
    public GameObject regularCam;
    public GameObject playerIn;
    // Start is called before the first frame update
    void Start()
    {
        tileBarrier.SetActive(false);
        print("Trigger");
        tileBarrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(tileBarrier.activeSelf == true && enemiesInside.Count == 0)
        {
            tileBarrier.SetActive(false);
            if (bossCam != null)
            {
                bossCam.SetActive(false);
                regularCam.SetActive(true);
            }
        }
        /*
        if (enemiesInside.Contains(null))
        {
            enemiesInside.Remove(null);
        }*/
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        print("Trigger Detected " + other.gameObject.CompareTag("Player"));
        if(other.gameObject.CompareTag("Player"))
        {
            if(spawnBoss != null)
            {
                var a = Instantiate(spawnBoss, transform.position, spawnBoss.transform.rotation);
                //enemiesInside.Add(a);
            }
            playerIn = other.gameObject;
            tileBarrier.SetActive(true);
            if(bossCam != null)
            {
                bossCam.SetActive(true);
                regularCam.SetActive(false);
            }
        }
        else if((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Item"))&& (!enemiesInside.Contains(other.gameObject)))
        {
            print("Count " + enemiesInside.Count);
            enemiesInside.Add(other.gameObject);
            if(playerIn != null)
            {
                tileBarrier.SetActive(true);
                if (bossCam != null)
                {
                    bossCam.SetActive(true);
                    regularCam.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (enemiesInside.Contains(other.gameObject))
        {
            enemiesInside.Remove(other.gameObject);
        } 
    }
}
