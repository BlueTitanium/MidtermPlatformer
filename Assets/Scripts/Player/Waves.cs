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
    public bool isEnabled = false;
    private GameManager gm;
    public AudioClip atckSound;
    public AudioClip spAtckSound;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerController>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.paused && attackTimeLeft > 0)
        {
            attackTimeLeft -= Time.unscaledDeltaTime;
        }
        if(!gm.paused && specialTimeLeft > 0)
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
            GetComponent<Animator>().SetTrigger("pattackboombox");
            
            StartCoroutine(spawn(.05f));
            //fix spawn point
            attackTimeLeft = attackCD;
        }
        
    }
    public IEnumerator spawn(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        p.SFX.PlayOneShot(atckSound);
        print("MAGIC BLAST");
        FindObjectOfType<CameraShaker>().ShakeCamera(1f, .3f);
        var a = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
        a.GetComponent<Projectile>().moveDirection((shootPoint.position - rotationPoint.position).normalized);
    }
    public IEnumerator spawnMany(float time, float delay, int count)
    {
        yield return new WaitForSecondsRealtime(time);
        p.SFX.PlayOneShot(spAtckSound);
        FindObjectOfType<CameraShaker>().ShakeCamera(.6f, .3f);
        for(;;)
        {
            var a = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
            a.GetComponent<Projectile>().moveDirection((shootPoint.position - rotationPoint.position).normalized);
            yield return new WaitForSecondsRealtime(delay);
            if(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "P_spattack_boombox")
            {
                continue;
            } else
            {
                break;
            }
        }
        
    }
    public override void Special()
    {
        if (specialTimeLeft <= 0)
        {
            base.Special();
            print("Laserbeam");
            GetComponent<Animator>().SetTrigger("spattackboombox");

            StartCoroutine(spawnMany(.05f,.01f,60));
            specialTimeLeft = specialCD;
            
        }
        

    }
    
    //double jump special;
    public override void Enable()
    {
        base.Enable();
        p.maxJumps = 2;
        isEnabled = true;
    }
    public override void Disable()
    {
        base.Disable();
        p.maxJumps = 1;
        isEnabled = false;
    }
}
