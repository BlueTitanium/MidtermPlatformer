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

    private bool canShoot = true;
    public Transform shootPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if(distToPlayer <= range) {
            if(canShoot){
                StartCoroutine(Shoot());
            }
        }

    }

    IEnumerator Shoot() 
    {   
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * shootSpeed * Time.fixedDeltaTime, 0f);
        print(newBullet.GetComponent<Rigidbody2D>().velocity);
        canShoot = true;
    }
}
