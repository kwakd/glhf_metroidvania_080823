// https://www.youtube.com/watch?v=rAnVMkJjWSI&list=PLgXA5L5ma2BvEqzzeLnb7Q_4z8bz_cKmO&index=2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float jumpForce = 25;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private float groundCheckY = 0.2f;
    
    private float xAxis;

    public static playerController instance;

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
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        playerMove();
        playerJump();
        playerFlip();
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

    public bool isPlayerGrounded()
    {
        if(Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) 
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void playerJump()
    {
        //better jump logic
        if(Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
        }

        if(Input.GetButtonDown("Jump") && isPlayerGrounded())
        {
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpForce);
        }

        myAnimator.SetBool("isJumpingPara", !isPlayerGrounded());
    }
}

