using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    [SerializeField]private float walkSpeed = 5;
    [SerializeField]private float jumpForce = 25;
    [SerializeField]private float groundCheckX = 0.5f;
    [SerializeField]private float groundCheckY = 0.2f;
    [SerializeField]private Transform groundCheckPoint;
    [SerializeField]private LayerMask whatIsGround;

    private float xAxis;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        movePlayer();
        playerJump();
    }

    void getInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void movePlayer()
    {
        myRigidbody.velocity = new Vector2(walkSpeed * xAxis, myRigidbody.velocity.y);
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
    }
}
