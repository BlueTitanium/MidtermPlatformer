using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float dieTime, damage;
    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(CountDownTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground")){
            if(other.gameObject.CompareTag("Player"))
            {   
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            }
            Die();
        }
    }
    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(dieTime);
        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
