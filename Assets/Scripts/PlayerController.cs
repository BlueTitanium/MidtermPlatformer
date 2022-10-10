using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        MENU (START BUTTON)
        
        COMBAT STYLES maybe use a different class for this to handle it:
        - Noise
            - Blasts
        - Light
            - Sword

        SKILL TREE AVAILABLE TO MODIFY THE COMBAT STYLES

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

    [Header("Running")]
    public float speed = 15f;

    [Header("Jumping")]
    public int maxJumps = 1;
    public int curJumps = 0;
    public float jumpVelocity = 5f;
    public bool isJumping = false;

    [Header("Dashing")]
    public Vector2 dashSpeedMod = new Vector2(3f, 1.4f);
    private Vector2 dashDir;
    public float dashCD = 0.5f;
    public float dashCDLeft = 0f;
    public float dashLength = 0.5f;
    public float curDash = 0f;
    public bool isDashing = false;

    [Header("Weapons")]
    public int curIndex = 0; //0 sword, 1 waves
    public int curLength = 1;
    public Weapon[] weapons;
    private Weapon weapon;


    // Start is called before the first frame update
    void Start()
    {
        var actionmap = playerControls.FindActionMap("Gameplay");
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

        rb = GetComponent<Rigidbody2D>();
        Physics2D.gravity = normGravity * gravityMod;

        weapon = weapons[curIndex];
    }

    private void SwitchBTN_performed(InputAction.CallbackContext obj)
    {
        curIndex +=1;
        if (curIndex >= curLength)
        {
            curIndex = 0;
        }
        switch (curIndex)
        {
            case 0:
                
                weapon = weapons[curIndex];
                break;
            case 1:
                weapon = weapons[curIndex];
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
        if(!isDashing && dashCDLeft <= 0f)
        {
            isDashing = true;
            dashCDLeft = dashLength + dashCD;
            curDash = dashLength;
            dashDir = dir;
            if(dir == Vector2.zero)
            {
                dashDir.x = 1f;
            }
        }
    }

    private void OnJumpFinished(InputAction.CallbackContext obj)
    {
        isJumping = false;
        
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
        
    }

    // Update is called once per frame
    void Update()
    {
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
        } else
        {
            isDashing = false;
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
        if (collision.gameObject.CompareTag("Ground") && !isJumping)
        {
            curJumps = maxJumps;
        }
    }

    public void TakeDamage(float damage)
    {

    }

}
