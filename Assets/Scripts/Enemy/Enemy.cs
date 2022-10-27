using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP = 10;
    public float hp = 10;
    public float canTakeDamage = 0f;
    public GameObject deathEffect;
    private bool deathCommenced = false;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }
    private bool ContainsParam(Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (canTakeDamage > 0) {
            canTakeDamage -= Time.unscaledDeltaTime; 
            
        }

    }

    public void TakeDamage(float x, Quaternion rot)
    {
        Animator a = GetComponent<Animator>();
        
        print(canTakeDamage);
        if (canTakeDamage <= 0)
        {   
            print("Damage");
            if (a != null)
                a.SetTrigger("Damaged");
            hp -= x;
            canTakeDamage = .25f;

        }
        if(hp <= 0)
        {
            
            Instantiate(deathEffect, transform.position, rot);
            if (ContainsParam(a, "Death")){
                if (deathCommenced == false)
                {
                    a.SetTrigger("Death");
                    deathCommenced = true;
                }
            } else
            {
                Destroy(gameObject);
            }
            
        }
        
    }

    
}
