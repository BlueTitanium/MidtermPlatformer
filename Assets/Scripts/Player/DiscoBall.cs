using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : Weapon
{
    
    private PlayerController p;
    public float attackCD = 1f;
    public float attackTimeLeft = 0f;
    public float specialCD = 5f;
    public float specialTimeLeft = 0f;
    public bool isEnabled = false;
    
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
        if (isEnabled)
        {
            p.attackCDIndicator.fillAmount = (attackCD - attackTimeLeft) / attackCD;
            p.spattackCDIndicator.fillAmount = (specialCD - specialTimeLeft) / specialCD;
        }
    }

    public override void Attack()
    {
        if(attackTimeLeft <= 0)
        {
            base.Attack();
            print("Disco Strike");
            GetComponent<Animator>().SetTrigger("pattackdisco");
            attackTimeLeft = attackCD;
        }
        
    }
    public override void Special()
    {
        if (specialTimeLeft <= 0)
        {
            base.Special();
            print("Disco Explosion");
            GetComponent<Animator>().SetTrigger("spattackdisco");
            specialTimeLeft = specialCD;
            
        }
        

    }
    
    //double jump special;
    public override void Enable()
    {
        base.Enable();
        p.gravSwitchable = true;
        isEnabled = true;
    }
    public override void Disable()
    {
        base.Disable();
        p.gravSwitchable = false;
        if (p.gravSwitched)
        {
            p.SwitchGravity();
        }
        isEnabled = false;
    }
}
