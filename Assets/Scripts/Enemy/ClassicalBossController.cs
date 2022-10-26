using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassicalBossController : MonoBehaviour
{


    /// <summary>
    /// states: 
    /// - 
    /// - idle
    ///     - move parent towards target 
    /// - slashing
    /// - thrusting 
    /// </summary>

    private Rigidbody2D rb;

    public Transform target;
    public Transform parent;

    public Enemy e;
    public GameObject BOSSUI;
    public Image bossHPUI;
    public GameObject drop;

    public bool isDead = false;
    public float distToPlayer;
    public float range = 4f;
    public float speed;
    private float ogSpeed;

    public float cdTimeLeft = 0f;
    public float cdTime = .6f;

    public int maxNum = 2;
    private Animator a;
    private AudioSource aud;
    public AudioClip slash;
    public AudioClip thrust;
    // Start is called before the first frame update
    void Start()
    {
        BOSSUI.SetActive(false);
        rb = parent.GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerController>().transform;
        BeginFight();
        ogSpeed = speed;
        a = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    public void BeginFight()
    {
        BOSSUI.SetActive(true);
    }
    public void EndFight()
    {
        isDead = true;
        e.hp = 0;
        //SPAWN VIOLIN
        if(drop!= null)
        {
            Instantiate(drop, transform.position, drop.transform.rotation);
        }
        rb.gravityScale = 1f;
        
        StartCoroutine(Die(1));
    }

    public IEnumerator Die(float time)
    {
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .5f);
        yield return new WaitForSecondsRealtime(0.1f);
        FindObjectOfType<CameraShaker>().ShakeCamera(6, .1f);
        yield return new WaitForSecondsRealtime(time);
        BOSSUI.SetActive(false);
        Destroy(parent.gameObject);
    }
    public void TryAttack()
    {
        if(cdTimeLeft <= 0)
        {
            int curAttack = Random.Range(0, maxNum);
            switch (curAttack)
            {
                case 0:
                    a.SetTrigger("Slash");
                    aud.PlayOneShot(slash);
                    break;
                case 1:
                    a.SetTrigger("Thrust");
                    aud.PlayOneShot(thrust);
                    break;
                default:
                    break;
            }
            
            cdTimeLeft = cdTime;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if(cdTimeLeft > 0)
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
            if (-dir.x > 0)
            {
                parent.localScale = new Vector3(-1, parent.localScale.y, 1);
            }
            else
            {
                parent.localScale = new Vector3(1, parent.localScale.y, 1);
            }
            if (distToPlayer <= .9f)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = -dir.normalized * speed;
            }
            if(distToPlayer <= range)
            {
                TryAttack();
            }
        }
    }
}
