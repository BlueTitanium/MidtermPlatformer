using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    /*
        CONTROLS DESIGN:
        MOVEMENT
        - RUNNING (LEFT AND RIGHT)
        - JUMPING (X BUTTON ps4)
           - should also have wall sliding/jumping
        - DIRECTIONAL DASH (TRIGGER + JOYSTICK DIRECTION)
            Default dash into direction of looking
        COMBAT
        - ATTACK/INTERACT ([] ps4)
        - SPECIAL ATTACK (triangle ps4)
        - SWITCH COMBAT STYLE (o ps4)
            - Maybe open menu
        MENU (START BUTTON)
        
        COMBAT STYLES maybe use a different class for this to handle it:
        - Noise
            - Blasts
        - Light
            - Sword

        Has HP / Regenerating MP pool
        Special Meter
        
     */
    [Header("Globals")]
    [Space] [SerializeField] private InputActionAsset playerControls;
    public float maxHP = 10f;
    public float curHP = 10f;
    public Image healthBar;
    public float canTakeDamage = 0f;
    public float gravityMod = 1f;
    private Rigidbody2D rb;
    private Vector2 normGravity = new Vector2(0, -9.81f);
    private Animator anim;
    private InputAction direction;
    private Vector2 dir;
    private Vector2 curVelocity;
    private GameManager gm;
    public InputActionMap actionmap;
    public GameObject rotationPoint;
    public Animation playerUIAnim;

    [Header("Audio")]
    public AudioSource SFX;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;

    [Header("Running")]
    public float speed = 15f;

    [Header("Jumping")]
    public int maxJumps = 1;
    public int curJumps = 0;
    public float jumpVelocity = 5f;
    public bool isJumping = false;
    public Transform GroundCheck; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.
    public Image jumpIndicator;
    public TextMeshProUGUI jumpsLeftText;

    [Header("Dashing")]
    public Vector2 dashSpeedMod = new Vector2(3f, 1.4f);
    private Vector2 dashDir;
    public bool bladedDash = false;
    public GameObject bladedDashHitbox;
    public float dashCD = 0.5f;
    public float dashCDLeft = 0f;
    public float dashLength = 0.5f;
    public float curDash = 0f;
    public bool isDashing = false;
    public bool dashResettable = false;
    public bool isSprinting;
    public DashTrail dashTrail;
    public bool gravSwitched = false;
    public bool gravSwitchable = false;
    public Image dashCDIndicator;

    [Header("Weapons")]
    public int curIndex = 0; //0 sword, 1 waves
    public int curLength = 1;
    public Weapon[] weapons;
    private Weapon weapon;
    public Image[] imageBackgrounds;
    public Color[] colors;
    public Image attackCDIndicator;
    public Image spattackCDIndicator;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        levelManager = GameObject.FindObjectOfType<LevelManager>();

        curHP = maxHP;
        gm = FindObjectOfType<GameManager>();
        actionmap = playerControls.FindActionMap("Gameplay");
        
        actionmap.Enable();

        direction = actionmap.FindAction("MOVEMENT");
        direction.performed += OnDirectionChanged;
        direction.canceled += OnDirectionChanged;

        var jump = actionmap.FindAction("JUMP");
        jump.started += OnJumpPress;
        jump.performed += OnJumpFinished;
        jump.canceled += OnJumpFinished;

        var dash = actionmap.FindAction("DASH");
        dash.performed += Dash_performed;

        var attack = actionmap.FindAction("ATTACK");
        attack.performed += Attack_performed;

        var special = actionmap.FindAction("SPECIAL");
        special.performed += Special_performed;

        var switchBTN = actionmap.FindAction("SWITCH");
        switchBTN.performed += SwitchBTN_performed;

        var menuBTN = actionmap.FindAction("MENU");
        menuBTN.performed += MenuBTN_performed;

        //TODO: implement the quickswitch buttons
        var switch1 = actionmap.FindAction("1");
        var switch2 = actionmap.FindAction("2");
        var switch3 = actionmap.FindAction("3");
        var switch4 = actionmap.FindAction("4");
        switch1.performed += Switch1_performed;
        switch2.performed += Switch2_performed;
        switch3.performed += Switch3_performed;
        switch4.performed += Switch4_performed;

        var RESET = actionmap.FindAction("RESET");
        RESET.performed += RESET_performed;

        rb = GetComponent<Rigidbody2D>();
        Physics2D.gravity = normGravity * gravityMod;
        bladedDashHitbox.SetActive(false);
        weapon = weapons[curIndex];
        print(curIndex);
        weapon.Enable();
        for(int i = 0; i < curLength; i++)
        {
            imageBackgrounds[i].color = colors[0];
        }
        imageBackgrounds[curIndex].color = colors[1];

    }

    private void RESET_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            Restart();
        }
    }

    private void Switch4_performed(InputAction.CallbackContext obj)
    {
        if (this != null && curLength > 3)
        {
            weapon.Disable();
            imageBackgrounds[curIndex].color = colors[0];
            curIndex = 3;
            imageBackgrounds[curIndex].color = colors[1];
            weapon = weapons[curIndex];
            weapon.Enable();
            if (levelManager != null)  levelManager.weaponEquipped = curIndex;
        }
    }

    private void Switch3_performed(InputAction.CallbackContext obj)
    {
        if (this != null && curLength > 2)
        {
            weapon.Disable();
            imageBackgrounds[curIndex].color = colors[0];
            curIndex = 2;
            imageBackgrounds[curIndex].color = colors[1];
            weapon = weapons[curIndex];
            weapon.Enable();
            if (levelManager != null) levelManager.weaponEquipped = curIndex;
        }
    }

    private void Switch2_performed(InputAction.CallbackContext obj)
    {
        if (this != null && curLength > 1)
        {
            weapon.Disable();
            imageBackgrounds[curIndex].color = colors[0];
            curIndex = 1;
            imageBackgrounds[curIndex].color = colors[1];
            weapon = weapons[curIndex];
            weapon.Enable();
            if(levelManager != null)
                levelManager.weaponEquipped = curIndex;
        }
    }

    private void Switch1_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            weapon.Disable();
            imageBackgrounds[curIndex].color = colors[0];
            curIndex = 0;
            imageBackgrounds[curIndex].color = colors[1];
            weapon = weapons[curIndex];
            weapon.Enable();
            if (levelManager != null) levelManager.weaponEquipped = curIndex;
        }
    }

    private void MenuBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            gm.Pause();
            actionmap.Disable();
        }
    }

    private void SwitchBTN_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            weapon.Disable();
            imageBackgrounds[curIndex].color = colors[0];
            curIndex += 1;
            if (curIndex >= curLength)
            {
                curIndex = 0;
            }
            imageBackgrounds[curIndex].color = colors[1];
            weapon = weapons[curIndex];
            weapon.Enable();
            if (levelManager != null) levelManager.weaponEquipped = curIndex;
        }
    }

    private void Special_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            weapon.Special();
        }
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            weapon.Attack();
        }
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            if (!gravSwitchable)
            {
                if (!isDashing && dashCDLeft <= 0f && !isSprinting)
                {
                    if (bladedDash)
                    {
                        print("on");
                        bladedDashHitbox.SetActive(true);
                    }
                    SFX.PlayOneShot(dashSound);
                    isDashing = true;
                    dashTrail.mbEnabled = true;
                    dashCDLeft = dashLength + dashCD;
                    curDash = dashLength;
                    dashDir = dir;

                    if (dir == Vector2.zero)
                    {
                        dashDir.x = 1f * transform.localScale.x;
                    }

                }
            }
            else
            {
                SwitchGravity();
            }
        }
        
    }
    public void SwitchGravity()
    {
        if (gravSwitched)
        {
            rb.gravityScale = 1;
            transform.localScale = new Vector3(transform.localScale.x, 1);
            gravSwitched = false;
        }
        else
        {
            gravSwitched = true;
            rb.gravityScale = -1f;
            transform.localScale = new Vector3(transform.localScale.x, -1);
        }
    }

    private void OnJumpFinished(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            isJumping = false;
            curVelocity = new Vector2(curVelocity.x, 0);
        }
    }

    private void OnJumpPress(InputAction.CallbackContext obj)
    {
        if(this != null)
        {
            if (curJumps > 0)
            {
                SFX.PlayOneShot(jumpSound);
                GetComponent<Animator>().SetTrigger("Jumping");
                curVelocity = new Vector2(curVelocity.x, jumpVelocity * transform.localScale.y);
                rb.velocity = curVelocity;
                isJumping = true;
                curJumps -= 1;
            }
        }
        
    }

    private void OnDirectionChanged(InputAction.CallbackContext context)
    {
        if (this != null)
        {
            dir = context.ReadValue<Vector2>();
            //I want to turn this direction value to an angle between 90 degrees and -90 degrees
            //so rotation of an arm can be seen
            if (dir.x > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y);
                rotationPoint.transform.eulerAngles = new Vector3(0, 0, Angle(dir));
            }
            else if (dir.x < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y);
                rotationPoint.transform.eulerAngles = new Vector3(0, 0, Angle(-dir));
            }
            else
            {
                if (dir.y > 0)
                {
                    rotationPoint.transform.eulerAngles = new Vector3(0, 0, 90 * transform.localScale.x);
                }
                else if (dir.y < 0)
                {
                    rotationPoint.transform.eulerAngles = new Vector3(0, 0, -90 * transform.localScale.x);
                }
            }
        }
        
    }

    public static float Angle(Vector2 vector2)
    {
        float angle;
        if (vector2.x < 0)
        {
            angle = 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            angle = Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
        angle *= -1;
        angle += 90;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        //UI STUFF
        healthBar.fillAmount = (curHP / maxHP);
        if (isSprinting)
        {
            dashCDIndicator.fillAmount = 0;
        }
        else
        {
            dashCDIndicator.fillAmount = ((dashCD - dashCDLeft) / dashCD);
        }
        jumpsLeftText.text = "JUMPS:\n" + curJumps;
        jumpIndicator.fillAmount = (float)curJumps / maxJumps;

        if (canTakeDamage > 0)
        {
            canTakeDamage -= Time.deltaTime;
        }
        var grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, groundLayer);
        if (grounded && !isJumping)
        {
            curJumps = maxJumps;
        }
        if (!grounded)
        {
            dashResettable = true;
            GetComponent<Animator>().SetBool("Grounded", false);
        } else
        {
            GetComponent<Animator>().SetBool("Grounded", true);
        }
        if(grounded && dashResettable)
        {
            dashResettable = false;
            dashCDLeft = 0f;
        }
        if (isJumping)
        {
            curVelocity = new Vector2(0, curVelocity.y);
        }
        else
        {
            curVelocity = new Vector2(0, rb.velocity.y);
        }

        if (curDash > 0)
        {
            if (bladedDash)
            {
                bladedDashHitbox.transform.localPosition = Vector2.zero;
            }
            curDash -= Time.deltaTime;
            canTakeDamage = .01f;
        } else if(isDashing)
        {
            isDashing = false;
            dashTrail.mbEnabled = false;
            bladedDashHitbox.SetActive(false);
            curVelocity = new Vector2(0, 0);
        }
        if(dashCDLeft > 0)
        {
            dashCDLeft -= Time.deltaTime;
        }
        MoveDirection();
        if(rb.velocity == Vector2.zero)
        {
            GetComponent<Animator>().SetBool("Running", false);
        } else
        {
            GetComponent<Animator>().SetBool("Running", true);
        }
    }

    void MoveDirection()
    {
        if (!isDashing)
        {
            curVelocity = new Vector2(dir.x * speed, curVelocity.y);

        }
        else
        {
            
            curVelocity = dashDir * speed * dashSpeedMod;
        }
        rb.velocity = curVelocity;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("killBox")){
            Restart();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    

    public void TakeDamage(float damage)
    {
        if (canTakeDamage <= 0 && !isDashing)
        {
            SFX.PlayOneShot(hurtSound);
            FindObjectOfType<CameraShaker>().ShakeCamera(2f, .4f);
            curHP -= damage;
            canTakeDamage = .2f;
            GetComponent<Animator>().SetTrigger("Damaged");
        }
        
        if (curHP <= 0)
        {
            SFX.PlayOneShot(hurtSound,.5f);
            FindObjectOfType<CameraShaker>().ShakeCamera(2f, .4f);
            GetComponent<Animator>().SetTrigger("Damaged");
            Restart();
        }
    }

    public void Restart()
    {
        if(levelManager!=null)
            levelManager.weaponLength = curLength;
        actionmap.Disable();
        StartCoroutine(RestartLevel());
    }
    public IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
