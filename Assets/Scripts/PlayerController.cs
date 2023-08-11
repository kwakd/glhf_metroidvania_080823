// https://www.youtube.com/watch?v=OepGkSuhqJg&list=PLgXA5L5ma2BvEqzzeLnb7Q_4z8bz_cKmO&index=3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] GameObject dashEffect;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float jumpForce = 25;
    [SerializeField] private float dashSpeed = 70;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.35f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float coyoteTime;
    [SerializeField] private int jumpBufferFrames;
    [SerializeField] private int maxAirJumps;
    
    private float xAxis;
    private float coyoteTimeCounter = 0;
    private float gravity;
    private int jumpBufferCounter = 0;
    private int airJumpCounter = 0;

    private bool dashed;
    private bool canDash = true;

    public static playerController instance;
    playerState pState;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<playerState>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        gravity = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        updateJumpVariables();
        if(pState.dashing) return;
        playerFlip();
        playerMove();
        playerJump();
        startDash();
    }

    void getInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    void playerFlip()
    {
        if(xAxis < 0)
        {
            transform.localScale = new Vector2(-1.5f, transform.localScale.y);
        }
        else if(xAxis > 0)
        {
            transform.localScale = new Vector2(1.5f, transform.localScale.y);
        }
    }

    private void playerMove()
    {
        myRigidbody.velocity = new Vector2(walkSpeed * xAxis, myRigidbody.velocity.y);
        myAnimator.SetBool("isWalkingPara", myRigidbody.velocity.x != 0 && isPlayerGrounded());
    }

    void startDash()
    {
        if(Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(playerDash());
            dashed = true;
        }
        if(isPlayerGrounded())
        {
            dashed = false;
        }
    }

    IEnumerator playerDash()
    {
        canDash = false;
        pState.dashing = true;
        myAnimator.SetTrigger("isDashingPara");
        myAnimator.SetBool("isDashingBoolPara", true);
        myRigidbody.gravityScale = 0;
        myRigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if(isPlayerGrounded())
        {
            Instantiate(dashEffect, transform);
        }
        yield return new WaitForSeconds(dashTime);
        myRigidbody.gravityScale = gravity;
        pState.dashing = false;
        myAnimator.SetBool("isDashingBoolPara", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool isPlayerGrounded()
    {
        if(Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) 
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            myAnimator.SetBool("isGroundedPara", true);
            return true;
        }
        else
        {
            myAnimator.SetBool("isGroundedPara", false);
            return false;
        }
    }

    void playerJump()
    {
        //better jump logic
        if(Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
            pState.jumping = false;
        }

        if(!pState.jumping)
        {
            if(jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpForce);
                pState.jumping = true;
            }
            else if(!isPlayerGrounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter++;
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpForce);
            }
        }

        myAnimator.SetBool("isJumpingPara", !isPlayerGrounded());
    }

    void updateJumpVariables()
    {
        if(isPlayerGrounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}

// TODO
    // movement: add fast falling
    // movement: dash
    // movement: dive (jump -> dive)
    // movement: double jump
    // movement: wall jump
    // player: attack
    // player: hp
    // enemy: goomba like enemy
    // enemy: shooting enemy?
    // enemy: jumping enemy
    // enemy: boss 1
    // UI: hp
    // UI: menu
    // UI: pause
    // UI: inventory(?)
    // stage: main hub
    // stage: level 1

// DONE
// MAKE BETTER


