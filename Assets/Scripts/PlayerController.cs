using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementInputDirection;
    private float movementInputDirectionVert;
    public float currentGravity;


    public float respawnLevel = -15F;
    public Vector3 StartingPosition;

    private int amountOfJumpsLeft;
    public int facingDirection = 1;

    //Booleans for control and animation
    private bool isFacingRight = true;
    public bool isWalking;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isLedgeGrab;
    private bool isWallSliding;
    public bool isWallHanging;
    public bool isHanging;
    public bool isJumping;
    public bool canJump;
    private bool expJump;
    public bool isFiring;
    public bool isAngle;
    //**********************************

    private Rigidbody2D rb;
    private Animator anim;
    //private Angle angle;
    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float ledgeCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        currentGravity = rb.gravityScale;
        StartingPosition = rb.position;
        //angle = new Angle();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckOutOfBounds();
        CheckHanging();
        CheckWallHaning();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckOutOfBounds()
    {
        if (rb.position.y < respawnLevel)
        {
            rb.position = StartingPosition;
        }
    }
    private void CheckHanging()
    {
        if (isLedgeGrab && !isGrounded && rb.velocity.y <= 0 && (!isWallSliding && !isWallHanging))
        {
            isJumping = false;
            isHanging = true;
        }
        else
        {
            isHanging = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canJump", canJump);
        anim.SetBool("isHanging", isHanging);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallHanging", isWallHanging);
        //anim.SetFloat("movementDirection",movementInputDirection);
        anim.SetBool("isTouchingWall", isTouchingWall);
        anim.SetBool("expJump", expJump);
        anim.SetBool("isAngle", isAngle);
        anim.SetBool("isFiring", isFiring);
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y <= 0 && isHanging)
        {
            isWallSliding = true;
        }
        

        else
        {
            isWallSliding = false;
        }

        if (isTouchingWall && !isHanging)
        {
            Debug.Log("I'm here, fortuntely.");
            Vector2 newForce = new Vector2(rb.velocity.x, -rb.velocity.y);
            rb.AddForce(newForce);
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        isLedgeGrab = Physics2D.Raycast(ledgeCheck.position, transform.right, ledgeCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0) || isWallSliding)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            isJumping = true;
            canJump = false;
        }
        else
        {
            isJumping = false;
            canJump = true;
        }

    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0)
        {
            if (isGrounded)
                isWalking = true;
        }
        else
        {
            isWalking = false;
        }

    }
    private void CheckWallHaning()
    {
        if (isWallSliding && movementInputDirection == facingDirection)
        {
            isWallHanging = true;
        }
        else
        {
            isWallHanging = false;
        }

    }


    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        movementInputDirectionVert = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {

            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAngle = true;
            PlayerAngle.setAngle(true);


        }
        if (Input.GetMouseButtonUp(1))
        {
            isAngle = false;
            PlayerAngle.setAngle(false);
        }

    }
    private void Fire()
    {
        if (!isHanging && !isWallSliding)
        {
            isFiring = true;
        }

    }
    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
        else if (isWallSliding && movementInputDirection == 0 && canJump) //Wall hop
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if ((isWallSliding || isTouchingWall) && movementInputDirection != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if (isHanging)
        {
            Vector2 forceToAdd = new Vector2(0, 30.0f);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    private void ApplyMovement()
    {
        if (isWallSliding)
        {
            isHanging = false;
        }
        //	If Player is touching the ground, move normally through the direction you're holding
        if (isGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }

        else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
        {
            // If Player is not grounded and is not sliding on a wall, and they're holding a direction,
            // add force in the direction they're holding
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            // If Player movement is faster than max movement speed, cap speed at max movement speed
            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }

        else if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            // If Player is not grounded and not wall sliding, and also not holding a direction,
            // Decrease horizontal velocity with respect to air drag
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }


        if (isWallSliding && isWallHanging)
        {
            // If Player is Wall Sliding and holding a direction towards the wall, disable gravity for Player
            // to have them 'stick' to the wall
            rb.gravityScale = 0;


        }
        else if (isWallSliding && !isWallHanging)
        {
            // If Player is Wall Sliding and not holding a direction towards the wall, have them slowly slip down

            rb.gravityScale = currentGravity;
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }

        }
        else if (isWallSliding && !isHanging && movementInputDirectionVert < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 2 * (-wallSlideSpeed));
        }
        else if (isHanging && !isWallSliding)
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;

        }
        else if (isHanging && expJump)
        {
            rb.AddForce(new Vector2(0, 10.0f));
            expJump = false;
        }

        else if (!isWallSliding && !isHanging)
        {
            // If player is no longer Wall Sliding, resume normal gravity
            rb.gravityScale = currentGravity;
        }


        if (isHanging && movementInputDirectionVert < 0)
        {
            isWallSliding = true;
            isHanging = false;
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {

            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }

}