using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlayerHitbox : MonoBehaviour
{
    public float damage = 5f;
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
            collision.GetComponent<Enemy>().TakeDamage(damage, transform.rotation);
            FindObjectOfType<CameraShaker>().ShakeCamera(.8f, .3f);
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hello!");
            collision.GetComponent<Enemy>().TakeDamage(damage, transform.rotation);
            FindObjectOfType<CameraShaker>().ShakeCamera(1f, .3f);
        }


    }
}
