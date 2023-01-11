using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        UserInput();
        PlayerMovement();
        Checkers();

        GroundReset();
    }

    void UserInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    } 

    void PlayerMovement()
    {
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

        // Player Jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        // Double Jump
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJumpChecker && !isGrounded)
        {
            DoubleJump();
        }

        // FastFall 
        if(Input.GetAxis("Vertical") < -0.5f && !isGrounded)
        {
            FastFall();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void DoubleJump()
    {
        Jump();
        doubleJumpChecker = false;
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
