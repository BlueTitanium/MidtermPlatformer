using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : Weapon
{
    private PlayerController p;
    public float attackCD = 1f;
    public float attackTimeLeft = 0f;
    public float specialCD = 5f;
    public float specialTimeLeft = 0f;
    public float jumpSpeedMod = 1.5f;
    public float runSpeedMod = 1.5f;
    public DashTrail sprintTrail;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimeLeft > 0)
        {
            attackTimeLeft -= Time.unscaledDeltaTime;
        }
        if (specialTimeLeft > 0)
        {
            specialTimeLeft -= Time.unscaledDeltaTime;
        }
    }

    public override void Attack()
    {
        if (attackTimeLeft <= 0)
        {
            base.Attack();
            print("Guitar Wave");
            GetComponent<Animator>().SetTrigger("pattackguitar");
            attackTimeLeft = attackCD;
        }

    }
    public override void Special()
    {
        if (specialTimeLeft <= 0)
        {
            base.Special();
            print("Guitar Shocker");
            specialTimeLeft = specialCD;

        }


    }

    //sprinting;
    public override void Enable()
    {
        base.Enable();
        print("Hello!");
        p.isSprinting = true;
        sprintTrail.mbEnabled = true;
        p.speed *= runSpeedMod;
        p.jumpVelocity *= runSpeedMod;
    }
    public override void Disable()
    {
        base.Disable();
        p.isSprinting = false;
        sprintTrail.mbEnabled = false;
        p.speed /= runSpeedMod;
        p.jumpVelocity /= runSpeedMod;

    }
}
