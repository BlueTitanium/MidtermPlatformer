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
    public GeneralPlayerHitbox hbox;
    public bool isEnabled = false;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "P_spattack_guitars")
        {
            hbox.reduceIFRAMES = true;
        } else
        {
            hbox.reduceIFRAMES = false;
        }
        if (!gm.paused && attackTimeLeft > 0)
        {
            attackTimeLeft -= Time.unscaledDeltaTime;
        }
        if (!gm.paused && specialTimeLeft > 0)
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
            GetComponent<Animator>().SetTrigger("spattackguitar");
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
        isEnabled = true;
    }
    public override void Disable()
    {
        base.Disable();
        p.isSprinting = false;
        sprintTrail.mbEnabled = false;
        p.speed /= runSpeedMod;
        p.jumpVelocity /= runSpeedMod;
        isEnabled = false;
    }
}
