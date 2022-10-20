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
    public Transform rotationPoint;
    public Transform shootPoint;
    public GameObject projectile;
    
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
            GetComponent<Animator>().SetTrigger("pattackboombox");
            
            StartCoroutine(spawn(.05f));
            //fix spawn point
            attackTimeLeft = attackCD;
        }
        
    }
    public IEnumerator spawn(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        print("MAGIC BLAST");
        var a = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
        a.GetComponent<Projectile>().moveDirection((shootPoint.position - rotationPoint.position).normalized);
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
