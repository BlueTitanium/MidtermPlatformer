using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayerHitbox : MonoBehaviour
{
    public float damage = 5f;
    public GameObject hitEffect;
    public bool reduceIFRAMES = false;
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
            if (reduceIFRAMES)
                collision.GetComponent<Enemy>().canTakeDamage = .05f;
            FindObjectOfType<CameraShaker>().ShakeCamera(1f, .3f);
        }


    }
}
