using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// void helperSnoopy()
//     {
//             string text =
//         @"  ,-~~-.___.
//         / |  ' 	\    	 
//         (  )     	0  
//         \_/-, ,----'       	 
//             ====       	//
//         /  \-'~;	/~~~(O)
//         /  __/~|   /   	|	 
//         =(  _____| (_________|
//             )
//         ";
//    	    Debug.Log(text);
//     }

//TODO: PlayerMovement
    //TODO: fastFalling
    //TODO: wallJump
    //TODO: faster running after a while
    //TODO: ATTACK LIKE IN HOLLOW KNIGHT
    //TODO: DASH LIKE IN AUTORUNNER
        //TODO: Think about changing direction stuff too
    //TODO: Coyote jumping
    //TODO: Jump Buffer
    //TODO: HOOKSHOT
//TODO: Player Health
//TODO: Items
//TODO: Inventory
//TODO: Game Manager
//TODO: Environment
//TODO: Stage
    //TODO: MainHub
    //TODO: STAGE 1
    //TODO: STAGE 1.5
    //TODO: SECRET ROOM
//TODO: Enemy1 - simple walking enemy (goomba)
//TODO: BOSS1 - BRAINSTORM
//TODO: SPEEDOMETER
//TODO: Add Sound
//TODO: MY OWN ASSETS

//======MAYBE TODO======
//TODO: PlayerMovement
    //TODO: diveDiag -> keep momentum
    //TODO: each jump makes player go faster?(triplejump mario) // faster running after a while?

//GOAL: MAIN HUB -> STAGE 1 -> MINIBOSS (PLAYER UNLOCKS) -> STAGE 1.5 -> BOSS -> SECRET ROOM

public class PlayerController : MonoBehaviour
{
    private float moveInput;

    private bool isGrounded;
    private bool doubleJumpChecker;
    private bool isPlayerLookingRight = true;

    private Rigidbody2D rb;

    public float moveSpeed;
    public float jumpForce;
    public float fastFallForce;
    public float checkRadius;

    public bool isInvincible;

    public Transform groundCheck;

    public LayerMask whatIsGround;
    //public LayerMask whatIsWall;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Checkers();
        UserInput();
        PlayerMovement();
        GroundReset();
    }

    void UserInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    } 

    void PlayerMovement()
    {
        //Move Player Horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        //Debug.Log(rb.velocity.x);

        // Flip player when moving left or right
        if(moveInput < 0f && isPlayerLookingRight)
        {
            Flip();
            
        }
        else if(moveInput > 0f && !isPlayerLookingRight)
        {
            Flip();
        }

        //Jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                Jump();
            }
            else if(doubleJumpChecker)
            {
                Jump();
                doubleJumpChecker = false;
            }
        }
        //for better jump logic (releasing jump)
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // FastFall 
        // if(Input.GetAxis("Vertical") < -0.5f && !isGrounded)
        // {
        //     FastFall();
        // }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void FastFall()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-fastFallForce);
    } 

    void Flip()
    {
        //wallJumpDirection *= -1;
        isPlayerLookingRight = !isPlayerLookingRight;
        transform.Rotate(0, 180, 0);
    } 

    void GroundReset()
    {
        if(isGrounded)
        {
            doubleJumpChecker = true;
        }
    } 

    void Checkers()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        //GROUNDCHECK
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }


}
