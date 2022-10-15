using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public float gravityMod = 1f;
    private Rigidbody2D rb;
    private Vector2 normGravity = new Vector2(0, -9.81f);
    private Animator anim;
    private InputAction direction;
    private Vector2 dir;
    private Vector2 curVelocity;
    private GameManager gm;
    public InputActionMap actionmap;

    [Header("Running")]
    public float speed = 15f;

    [Header("Jumping")]
    public int maxJumps = 1;
    public int curJumps = 0;
    public float jumpVelocity = 5f;
    public bool isJumping = false;
    public Transform GroundCheck; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.

    [Header("Dashing")]
    public Vector2 dashSpeedMod = new Vector2(3f, 1.4f);
    private Vector2 dashDir;
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

    [Header("Weapons")]
    public int curIndex = 0; //0 sword, 1 waves
    public int curLength = 1;
    public Weapon[] weapons;
    private Weapon weapon;
    public Image[] imageBackgrounds;
    public Color[] colors;


    // Start is called before the first frame update
    void Start()
    {
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

        rb = GetComponent<Rigidbody2D>();
        Physics2D.gravity = normGravity * gravityMod;

        weapon = weapons[curIndex];
        weapon.Enable();
        for(int i = 0; i < curLength; i++)
        {
            imageBackgrounds[i].color = colors[0];
        }
        imageBackgrounds[curIndex].color = colors[1];

    }

    private void MenuBTN_performed(InputAction.CallbackContext obj)
    {
        gm.Pause();
        actionmap.Disable();
    }

    private void SwitchBTN_performed(InputAction.CallbackContext obj)
    {
        weapon.Disable();
        imageBackgrounds[curIndex].color = colors[0];
        curIndex +=1;
        if (curIndex >= curLength)
        {
            curIndex = 0;
        }
        imageBackgrounds[curIndex].color = colors[1];
        switch (curIndex)
        {
            case 0:
                weapon = weapons[curIndex];
                weapon.Enable();
                break;
            case 1:
                weapon = weapons[curIndex];
                weapon.Enable();
                break;
            case 2:
                weapon = weapons[curIndex];
                weapon.Enable();
                break;
            case 3:
                weapon = weapons[curIndex];
                weapon.Enable();
                break;
            case 4:
                weapon = weapons[curIndex];
                weapon.Enable();
                break;
            default:
                break;
        }
    }

    private void Special_performed(InputAction.CallbackContext obj)
    {
        weapon.Special();
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        weapon.Attack();
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        if (!gravSwitchable)
        {
            if (!isDashing && dashCDLeft <= 0f && !isSprinting)
            {
                isDashing = true;
                dashTrail.mbEnabled = true;
                dashCDLeft = dashLength + dashCD;
                curDash = dashLength;
                dashDir = dir;
                if (dir == Vector2.zero)
                {
                    dashDir.x = 1f;
                }
            }
        } else
        {
            SwitchGravity();
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
            rb.gravityScale = -1;
            transform.localScale = new Vector3(transform.localScale.x, -1);
        }
    }

    private void OnJumpFinished(InputAction.CallbackContext obj)
    {
        isJumping = false;
        curVelocity = new Vector2(curVelocity.x, 0);
    }

    private void OnJumpPress(InputAction.CallbackContext obj)
    {
        if (curJumps > 0)
        {
            curVelocity = new Vector2(curVelocity.x, jumpVelocity);
            rb.velocity = curVelocity;
            isJumping = true;
            curJumps -= 1;
        }
    }

    private void OnDirectionChanged(InputAction.CallbackContext context)
    {
        Debug.Log($"X: {context.ReadValue<Vector2>().x}, Y: {context.ReadValue<Vector2>().y}");
        dir = context.ReadValue<Vector2>();
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y);
        } else if (dir.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        var grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, groundLayer);
        if (grounded && !isJumping)
        {
            curJumps = maxJumps;
        }
        if (!grounded)
        {
            dashResettable = true;
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
            curDash -= Time.deltaTime;
            
        } else if(isDashing)
        {
            isDashing = false;
            dashTrail.mbEnabled = false;
            curVelocity = new Vector2(0, 0);
        }
        if(dashCDLeft > 0)
        {
            dashCDLeft -= Time.deltaTime;
        }
        MoveDirection();
        
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
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    public void TakeDamage(float damage)
    {

    }

}
