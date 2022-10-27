using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LofiBossController : MonoBehaviour
{

    /// <summary>
    /// states: 
    /// 
    /// - idle
    ///     - move to standard pos
    /// 
    /// - shoot
    ///     - rotate towards target
    ///     
    /// - rising pencils 
    ///     - if hp below .5
    ///     - every 3 seconds in standard pos
    ///     
    /// - pencil swirl
    ///     - if player is close
    /// 
    /// - aggroing
    ///     - move to player and do pencil swirl 5 times before going back to idle
    ///    
    /// </summary>

    

    private Rigidbody2D rb;

    public Transform target;
    public Vector3 origin;
    public Transform parent;

    public Enemy e;
    public GameObject BOSSUI;
    public Image bossHPUI;
    public GameObject drop;
    public GameObject summonObject;

    public bool isDead = false;
    public float distToPlayer;
    public float range = 4f;
    public float speed;
    private float ogSpeed;

    public float cdTimeLeft = 0f;
    public float cdTime = .6f;

    public int maxNum = 2;
    private Animator a;
    bool isShooting = false;

    private AudioSource aud;
    public AudioClip shoot1;
    public AudioClip shoot2;
    public AudioClip whoosh;

    // Start is called before the first frame update
    void Start()
    {
        BOSSUI.SetActive(false);
        rb = parent.GetComponent<Rigidbody2D>();
        origin = parent.transform.position;
        target = FindObjectOfType<PlayerController>().transform;
        isDead = true;
        StartCoroutine(BeginFight());
        ogSpeed = speed;
        a = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
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

        if(drop != null)
            Instantiate(drop, transform.position, drop.transform.rotation);
        rb.gravityScale = 1f;

        StartCoroutine(Die(1));
    }

    public IEnumerator Die(float time)
    {
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .5f);
        yield return new WaitForSecondsRealtime(0.1f);
        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name != "LofiGirl_Death")
        {
            a.SetTrigger("Death");
        }
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .1f);
        yield return new WaitForSecondsRealtime(time);
        BOSSUI.SetActive(false);
        Destroy(parent.gameObject);
    }
    public void TryAttack()
    {
        if (cdTimeLeft <= 0)
        {
            int curAttack = Random.Range(0, maxNum);
            switch (curAttack)
            {
                case 0:
                    a.SetTrigger("Slash");
                    break;
                case 1:
                    a.SetTrigger("Thrust");
                    break;
                default:
                    break;
            }
            cdTimeLeft = cdTime;
        }
    }

    public void TryShield()
    {
        if (cdTimeLeft <= 0)
        {
            a.SetTrigger("ShieldOn");
            aud.PlayOneShot(whoosh);
            cdTimeLeft = .4f;
        }
    }

    public void TryShoot()
    {
        if (!isDead)
        {
            if (cdTimeLeft <= 0)
            {

                int curAttack = Random.Range(0, 4);
                print(curAttack);
                if (e.hp / e.maxHP < 0.5)
                {
                    print("second half");
                    switch (curAttack)
                    {
                        case 0:
                            a.SetTrigger("Shoot1");
                            aud.PlayOneShot(shoot1);
                            break;
                        case 1:
                            a.SetTrigger("SummonUp");
                            aud.PlayOneShot(shoot2);
                            break;
                        case 2:
                            a.SetTrigger("Shoot1");
                            aud.PlayOneShot(shoot1);
                            break;
                        case 3:
                            a.SetTrigger("Shoot1");
                            aud.PlayOneShot(shoot1);
                            break;
                        default:
                            break;
                    }
                }
                else
                {

                    print("first half");
                    if (summonObject.activeInHierarchy == false)
                    {
                        a.SetTrigger("SummonUp");
                        aud.PlayOneShot(shoot2);
                    }
                }
                cdTimeLeft = cdTime;
            }
        }
    }

    public IEnumerator SummonUp()
    {
        if (!isDead)
        {
            summonObject.SetActive(true);
            yield return new WaitForSeconds(1);
            summonObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LofiGirl_Shoot" || a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LofiGirl_SummonShot" || summonObject.activeInHierarchy)
        {
            isShooting = true;
        } else
        {
            isShooting = false;
        }
        if (cdTimeLeft > 0)
        {
            cdTimeLeft -= Time.deltaTime;
        }

        bossHPUI.fillAmount = e.hp / e.maxHP;
        if (e.hp / e.maxHP < 0.5)
        {
            speed = ogSpeed * 1.25f;
        }
        if (!isDead)
        {
            distToPlayer = Vector2.Distance(parent.position, target.position);
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
            
            if(distToPlayer <= range)
            {
                rb.velocity = Vector2.zero;
            }  else
            {
                if (e.hp / e.maxHP >= 0.5) rb.velocity = new Vector2(-dir2.normalized.x * speed, 0f);
                else rb.velocity = new Vector2(-dir.normalized.x * speed, 0f);
                //rb.velocity = -dir.normalized * speed;
            }
            if (e.hp / e.maxHP >= 0.5 && distToPlayer <= range)
            {
                TryShield();
            } else if (e.hp / e.maxHP < 0.5 && distToPlayer <= 4f)
            {
                TryShield();
            }
            else
            {
                TryShoot();
                
            }
        }
    }
}
