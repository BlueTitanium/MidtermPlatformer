using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private PlayerController p;
    
    private float oldDashLength;
    public float newDashLength = 1.4f;
    private Vector2 regularDashMod;
    public Vector2 dashModMod = new Vector2(3.5f,2.3f);
    public float attackCD = 1f;
    public float attackTimeLeft = 0f;
    public float specialCD = 5f;
    public float specialTimeLeft = 0f;

    public GameObject timeSlowGrayScale;
    public float timeSlowScale = .5f;
    public float curTimeSlow = 0f;
    public float timeSlowLength = 1f;
    public float properTimeScale = 1f;
    public bool isEnabled = false;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
        gm = FindObjectOfType<GameManager>();
        oldDashLength = p.dashLength;
        regularDashMod = p.dashSpeedMod;
        Time.timeScale = 1f;
        properTimeScale = 1f;
        timeSlowGrayScale.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (!gm.paused && curTimeSlow > 0)
        {
            curTimeSlow -= Time.unscaledDeltaTime;
        }
        
    }

    public override void Attack()
    {
        
        if (attackTimeLeft <= 0)
        {
            base.Attack();
            print("SWORD STRIKE");
            GetComponent<Animator>().SetTrigger("pattacksword");
            attackTimeLeft = attackCD;
        }
    }
    public override void Special()
    {
        
        if (specialTimeLeft <= 0)
        {
            base.Special();
            print("Time Slows");
            specialTimeLeft = specialCD;
            StartCoroutine(Timeslow());
        }

    }
    IEnumerator Timeslow()
    {
        //should player move faster during slowed time? leaning toward yes
        timeSlowGrayScale.SetActive(true);
        properTimeScale = timeSlowScale;
        Time.timeScale = properTimeScale;
        curTimeSlow = timeSlowLength;
        while (curTimeSlow > 0)
        {
            yield return null;
        }
        properTimeScale = 1f;
        Time.timeScale = properTimeScale;
        timeSlowGrayScale.SetActive(false);
    }
    //dash is different
    public override void Enable()
    {
        base.Enable();
        //modify dash length and speed
        //when dashing enable something that has a hitbox on dash
        p.dashLength = newDashLength;
        p.dashSpeedMod = dashModMod;
        p.bladedDash = true;
        isEnabled = true;
    }
    public override void Disable()
    {
        base.Disable();
        p.dashLength = oldDashLength;
        p.dashSpeedMod = regularDashMod;
        p.bladedDash = false;
        isEnabled = false;
    }
}
