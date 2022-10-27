using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscoBossController : MonoBehaviour
{
    /// <summary>
    /// states: 
    /// 
    /// - idle
    ///     - go to origin
    ///     - changes color constantly
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
    public TextMeshProUGUI bossHPText;
    public GameObject drop;

    public int colorIndex = 0;
    public Color[] colors;

    public GameObject Bullet;   
    public Transform dropPoint;

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
    public bool takingDamage = false;
    private AudioSource aud;
    public AudioClip swing;
    public AudioClip crash;
    public AudioClip spawn;
    // Start is called before the first frame update
    void Start()
    {
        BOSSUI.SetActive(false);
        rb = parent.GetComponent<Rigidbody2D>();
        origin = parent.transform.position;
        target = FindObjectOfType<PlayerController>().transform;
        isDead = true;
        GetComponent<SpriteRenderer>().color = colors[colorIndex];
        oldColor = colors[colorIndex];
        oldHealth = e.maxHP;
        StartCoroutine(BeginFight());
        ogSpeed = speed;
        a = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        
    }

    public void GoNextColor()
    {
        print("cindex: " + colorIndex);
        colorIndex = GetNextColor();
        GetComponent<SpriteRenderer>().color = colors[colorIndex];
        bossHPUI.color = colors[colorIndex];
        bossHPText.color = colors[GetNextColor()];
    }

    public int GetNextColor()
    {
        int next = colorIndex + 1;
        if (next >= colors.Length)
        {
            next = 0;
        }
        return next;
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
        oldColor = colors[colorIndex];
        takingDamage = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        
        yield return new WaitForSecondsRealtime(0.1f);
        takingDamage = false;
        e.canTakeDamage = 0;
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

                if (e.hp / e.maxHP > 0.9)
                {
                    int curAttack = 0;
                    print(curAttack);
                    print("first half");
                    a.SetTrigger("Swing1");
                }
                else
                {
                    int curAttack = Random.Range(0, 5);
                    print(curAttack);
                    print("Second Half");
                    switch (curAttack)
                    {
                        case 0:
                            a.SetTrigger("Swing2");
                            break;
                        case 1:
                            a.SetTrigger("Spawn");
                            break;
                        case 2:
                            a.SetTrigger("Swing1");
                            break;
                        case 3:
                            a.SetTrigger("Swing2");
                            break;
                        case 4:
                            a.SetTrigger("Spawn");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if(!takingDamage && !isDead)
            GetComponent<SpriteRenderer>().color = colors[colorIndex];
        else GetComponent<SpriteRenderer>().color = Color.red;
    }
    // Update is called once per frame
    void Update()
    {
        if (!takingDamage && !isDead)
            GetComponent<SpriteRenderer>().color = colors[colorIndex];
        else GetComponent<SpriteRenderer>().color = Color.red;
        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Disco_Idle")
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

        if (e.hp < oldHealth)
        {
            oldHealth = e.hp;
            StartCoroutine(DamageColor());

        }

        if (!isDead)
        {
            distToPlayer = Vector2.Distance(parent.position, origin);
            Vector2 dir = (parent.position - target.position);
            parent.transform.position = origin;
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
            
            TryShoot();
        }
    }

    public void SpawnEnemy()
    {
        //aud.PlayOneShot(spawn);
        Instantiate(Bullet, transform.position, Bullet.transform.rotation);

    }

    public void Swing()
    {
        //aud.PlayOneShot(swing);
        GoNextColor();
    }
    public void Crash()
    {
        //aud.PlayOneShot(crash);
        FindObjectOfType<CameraShaker>().ShakeCamera(2f, .4f);
    }
}
