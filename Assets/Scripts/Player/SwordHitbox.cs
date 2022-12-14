using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float damage = 5f;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hello!");
            if (collision.GetComponent<Enemy>().canTakeDamage <= 0)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            collision.GetComponent<Enemy>().TakeDamage(damage, transform.rotation);
            
            FindObjectOfType<CameraShaker>().ShakeCamera(.8f, .3f);
            FindObjectOfType<PlayerController>().dashCDLeft = 0;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hello!");
            if (collision.GetComponent<Enemy>().canTakeDamage <= 0)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            collision.GetComponent<Enemy>().TakeDamage(damage, transform.rotation);
            FindObjectOfType<CameraShaker>().ShakeCamera(.8f, .3f);
            FindObjectOfType<PlayerController>().dashCDLeft = 0;
        }
        //PARRY
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            print("deflected");
            Vector2 dir = collision.GetComponent<Rigidbody2D>().velocity;
            Vector2 pos = collision.transform.position;
            var a = Instantiate(FindObjectOfType<Waves>().projectile, pos, collision.transform.rotation);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            a.GetComponent<Projectile>().moveDirection(-dir.normalized);
            FindObjectOfType<CameraShaker>().ShakeCamera(1f, .3f);
            FindObjectOfType<PlayerController>().dashCDLeft = 0;
        }


    }
}
