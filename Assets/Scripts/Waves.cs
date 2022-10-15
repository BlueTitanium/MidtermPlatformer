using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : Weapon
{
    private PlayerController p;
    public float attackCD = 1f;
    public float attackTimeLeft = 0f;
    public float specialCD = 5f;
    public float specialTimeLeft = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackTimeLeft > 0)
        {
            attackTimeLeft -= Time.unscaledDeltaTime;
        }
        if(specialTimeLeft > 0)
        {
            specialTimeLeft -= Time.unscaledDeltaTime;
        }
    }

    public override void Attack()
    {
        if(attackTimeLeft <= 0)
        {
            base.Attack();
            print("MAGIC BLAST");
            attackTimeLeft = attackCD;
        }
        
    }
    public override void Special()
    {
        if (specialTimeLeft <= 0)
        {
            base.Special();
            print("Laserbeam");
            specialTimeLeft = specialCD;
            
        }
        

    }
    
    //double jump special;
    public override void Enable()
    {
        base.Enable();
        p.maxJumps = 2;
    }
    public override void Disable()
    {
        base.Disable();
        p.maxJumps = 1;
    }
}
