using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;
    public float damage = 5f;
    public float timeToDie = 4f;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDie);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveDirection(Vector3 dir)
    {
        rb.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage, transform.rotation);
            FindObjectOfType<CameraShaker>().ShakeCamera(2f, .3f);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            FindObjectOfType<CameraShaker>().ShakeCamera(.5f, .2f);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            FindObjectOfType<CameraShaker>().ShakeCamera(.5f, .2f);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
