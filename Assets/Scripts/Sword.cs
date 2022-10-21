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
    public float timeSlowLength = 1f;
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
        oldDashLength = p.dashLength;
        regularDashMod = p.dashSpeedMod;
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
        Time.timeScale = timeSlowScale;
        yield return new WaitForSecondsRealtime(timeSlowLength);
        Time.timeScale = 1f;
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
    }
    public override void Disable()
    {
        base.Disable();
        p.dashLength = oldDashLength;
        p.dashSpeedMod = regularDashMod;
        p.bladedDash = false;
    }
}