using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProjectileEnemy : MonoBehaviour {
    private Transform player;
    public float range;
    private float distToPlayer;
    public GameObject bullet;
    public float timeBetweenShots;
    public float direction;
    public float shootSpeed;

    //private bool canShoot = true;
    public Transform shootPos;

    public int beatsBetweenShots = 3;
    public int curBeat = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if(distToPlayer <= range) {
            if(canShoot){
                StartCoroutine(Shoot());
            }
        }*/
        
    }

    //below function will get called by animator so shooting will line up with animation
    public void TryShoot()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= range && curBeat == 0)
        {
            ShootNow();
            
        }
        curBeat++;
        if (curBeat == beatsBetweenShots)
        {
            curBeat = 0;
        }
    }

    
    public void ShootNow()
    {
        GetComponent<AudioSource>().Play();
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * shootSpeed * Time.fixedDeltaTime, 0f);
    }

    /*
    IEnumerator Shoot() 
    {   
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * shootSpeed * Time.fixedDeltaTime, 0f);
        //print(newBullet.GetComponent<Rigidbody2D>().velocity);
        canShoot = true;
    }*/
}
