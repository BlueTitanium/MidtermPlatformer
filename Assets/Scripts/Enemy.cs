using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 10;
    public float canTakeDamage = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canTakeDamage -= Time.unscaledDeltaTime;
    }

    public void TakeDamage(float x)
    {   
        print(canTakeDamage);
        if (canTakeDamage <= 0)
        {   
            print("Damage");
            hp -= x;
            canTakeDamage += .25f;
        }
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}