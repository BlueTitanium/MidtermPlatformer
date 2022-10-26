using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuitarBossController : MonoBehaviour
{


    /// <summary>
    /// states: 
    /// 
    /// - idle
    ///     - go to origin
    /// - attacking
    ///     - shootout1 has 1 function call
    ///     - shootout2 has 3 function calls
    ///     -
    /// - pingpong should follow target
    /// - damage
    /// </summary>



    private Rigidbody2D rb;

    public Transform target;
    public Vector3 origin;
    public Transform parent;

    public Enemy e;
    public GameObject BOSSUI;
    public Image bossHPUI;
    public GameObject drop;
    public GameObject Bullet;
    public Transform[] shooters1;
    public Transform[] shooters2;
    public Transform[] shooters3;
    public Transform dropPoint;
    public float shootSpeed;

    public bool isDead = false;
    public float distToPlayer;
    public float range = 4f;
    public float speed;
    private float ogSpeed;
    private Color oldColor;
    private float oldHealth;


    public float cdTimeLeft = 0f;
    public float cdTime = .6f;

    public int maxNum = 2;
    private Animator a;
    bool isShooting = false;
    public int isMoving = 0;

    private AudioSource aud;
    public AudioClip shoot;
    public AudioClip up;
    public AudioClip crash;
    public AudioClip whoosh; 
    // Start is called before the first frame update
    void Start()
    {
        BOSSUI.SetActive(false);
        rb = parent.GetComponent<Rigidbody2D>();
        origin = parent.transform.position;
        target = FindObjectOfType<PlayerController>().transform;
        isDead = true;
        oldColor = GetComponent<SpriteRenderer>().color;
        oldHealth = e.maxHP;
        StartCoroutine(BeginFight());
        ogSpeed = speed;
        a = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    
    public void playUpSound()
    {
        aud.PlayOneShot(up);
    }
    public void playCrashSound()
    {
        aud.PlayOneShot(crash);
    }
    public void playWhooshSound()
    {
        aud.PlayOneShot(whoosh);
    }

    public IEnumerator BeginFight()
    {

        yield return new WaitForSeconds(0.9f);
        BOSSUI.SetActive(true);
        isDead = false;
    }
    public void EndFight()
    {
        isDead = true;
        e.hp = 0;

        
        rb.gravityScale = 1f;

        StartCoroutine(Die(1));
    }

    public IEnumerator DamageColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<SpriteRenderer>().color = oldColor;
    }
    public IEnumerator Die(float time)
    {
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .5f);
        yield return new WaitForSecondsRealtime(0.1f);
        if (drop != null)
            Instantiate(drop, dropPoint.position, drop.transform.rotation);
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .1f);
        yield return new WaitForSecondsRealtime(time);
        BOSSUI.SetActive(false);
        Destroy(parent.gameObject);
    }
    public void TryShoot()
    {
        if (!isDead)
        {
            if (cdTimeLeft <= 0 && isShooting == false)
            {
                cdTimeLeft = cdTime;
                
                if (e.hp / e.maxHP > 0.5)
                {
                    int curAttack = Random.Range(0, 4);
                    print(curAttack);
                    print("first half");
                    switch (curAttack)
                    {
                        case 0:
                            a.SetTrigger("ShootOut");
                            break;
                        case 1:
                            a.SetTrigger("PingPong");
                            break;
                        case 2:
                            a.SetTrigger("ShootOut");
                            break;
                        case 3:
                            a.SetTrigger("PingPong");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    int curAttack = Random.Range(0, 3);
                    print(curAttack);
                    print("Second Half");
                    switch (curAttack)
                    {
                        case 0:
                            a.SetTrigger("ShootOut2");
                            break;
                        case 1:
                            a.SetTrigger("PingPong");
                            break;
                        case 2:
                            a.SetTrigger("Sweep");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }



    public void ShootNow(Transform shootPos)
    {
        
        GameObject newBullet = Instantiate(Bullet, shootPos.position, shootPos.rotation);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootPos.right.x * parent.localScale.x * shootSpeed, shootPos.right.y * parent.localScale.x * shootSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Guitar_Idle")
        {
            isShooting = false;
            isMoving = 0;
        }
        else
        {
            isShooting = true;
        }

        if (cdTimeLeft > 0)
        {
            cdTimeLeft -= Time.deltaTime;
        }

        bossHPUI.fillAmount = e.hp / e.maxHP;
        if (e.hp / e.maxHP < 0.5)
        {
            speed = ogSpeed * 3f;
        }

        if(e.hp < oldHealth)
        {
            oldHealth = e.hp;
            StartCoroutine(DamageColor());

        }

        if (!isDead)
        {
            distToPlayer = Vector2.Distance(parent.position, origin);
            Vector2 dir = (parent.position - target.position);
            Vector2 dir2 = (parent.position - origin);
            if (!isShooting)
            {
                if (-dir.x > 0)
                {
                    parent.localScale = new Vector3(-1, parent.localScale.y, 1);
                }
                else
                {
                    parent.localScale = new Vector3(1, parent.localScale.y, 1);
                }
            }
            switch (isMoving)
            {
                case 0:
                    rb.velocity = new Vector2(-dir2.normalized.x * speed, 0f);
                    if(distToPlayer <= 0.5f)
                    {
                        rb.velocity = new Vector2(0f, 0f);
                    }
                    if (!isShooting)
                    {
                        parent.transform.position = origin;
                    }
                    break;
                case 1:
                    rb.velocity = new Vector2(-dir.normalized.x * speed, 0f);
                    break;
                case 2:
                    rb.velocity = Vector2.zero;
                    break;
                default:
                    break;
            }
            TryShoot();
        }
    }

    public void StartMoving()
    {
        isMoving = 1;
    }
    public void EndMoving()
    {
        isMoving = 2;
    }
    public void goToOrigin()
    {
        isMoving = 0;
    }


    public void Shoot1()
    {
        aud.PlayOneShot(shoot);
        foreach (Transform t in shooters1)
        {
            ShootNow(t);
        }
    }
    public void Shoot2()
    {
        aud.PlayOneShot(shoot);
        foreach (Transform t in shooters2)
        {
            ShootNow(t);
        }
    }
    public void Shoot3()
    {
        aud.PlayOneShot(shoot);
        foreach (Transform t in shooters3)
        {
            ShootNow(t);
        }
    }
}
