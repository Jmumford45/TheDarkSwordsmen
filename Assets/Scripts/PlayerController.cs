using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform feet;
    public Transform wallCheck;

    private bool isFacingRight = true;
    private bool isMoving = false;
    private bool onGround = true;
    private bool canJump = true;
    private bool touchingWall;
    private bool isWallSliding;

    private float moveInputDir;
    [SerializeField] private float moveSpd = 4.5f;
    [SerializeField] private float jumpForce = 15.0f;
    [SerializeField] private float wallSlideSpeed = 2.0f;
    [SerializeField] private float forceInAir;
    [SerializeField] private float drag;
    [SerializeField] private float variable_jump_mult;

    private CheckCollisions collisions;

    public int maxJumps;
    private int numOfJumpsUsed;

    //don't keep this method, just an accessor for Camera script for now, may want to move stats to player scriptable object
    public float getSpeed() { return moveSpd; }
    public bool getIsMove() { return isMoving; }

    public bool getOnGround() { return onGround; }

    public float getYVelocity() { return rb.velocity.y; }

    public bool getWallSlide() { return isWallSliding; }

/*
 * jump behaviour: 
 * -add timer for when a second jump can be made, ?? check y velocity < 0 and timeb4nextjump < currenttime??????????????
 * 
 * wallSlide:
 * -if player is wallsliding and tries to jump add force in opposite direction
 * -??if player is wallsliding and tries to jump with oppositie direction either mimic above behaviour or add slight boost in jump??
 * 
 * Run:
 * 
 * Attack:
 */

private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    collisions = new CheckCollisions(feet, wallCheck);
    numOfJumpsUsed = maxJumps;
}

private void CheckInput()
{
    moveInputDir = Input.GetAxisRaw("Horizontal");

    if (moveInputDir != 0)
        isMoving = true;
    else
        isMoving = false;

    //onGround = collisions.CCollisions();
    if (Input.GetButtonDown("Jump") && canJump)
    {
        Jump();
    }
    if(Input.GetButtonUp("Jump"))
    {
        StopJump();
    }
}

private void CheckJump()
{
    if (onGround)
    {
        numOfJumpsUsed = maxJumps;
    }

    if (numOfJumpsUsed != 0)
    {
        canJump = true;
    }
    else
        canJump = false;
}

private void ApplyMovement()
{
    Vector2 walkForce = new Vector2(moveInputDir * moveSpd, rb.velocity.y);
    Vector2 forceToAdd = new Vector2(forceInAir * moveInputDir, 0);
    Vector2 dragV = new Vector2(drag * rb.velocity.x, rb.velocity.y);

    if (onGround)
    {
        rb.velocity = walkForce;
    }
    else if (!onGround)
    {
        if (isMoving)
        {
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > moveSpd)
            {
                rb.velocity = walkForce;
            }
        }
        else
        {
            rb.velocity = dragV;
        }

    }
        
    if (isWallSliding)
    {
        if (rb.velocity.y < -wallSlideSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }
    if(isWallSliding && !isMoving && !onGround)
    {
      PushOffWall();
    }
}

private void CheckMovementDir()
{
    if (moveInputDir < 0 && isFacingRight)
        Flip();
    else if (moveInputDir > 0 && !isFacingRight)
        Flip();
}

    private void Flip()
{
    isFacingRight = !isFacingRight;
    transform.Rotate(0.0f, 180.0f, 0.0f);
}

private void Jump()
{
    if (canJump)
    {
        if (!isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            numOfJumpsUsed--;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}

private void StopJump()
{
    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variable_jump_mult);
}

private void PushOffWall()
{
    Vector2 pushOffWall = new Vector2(forceInAir, 0);
    if (isFacingRight)
      rb.AddForce(pushOffWall * -1.0f);
    else
      rb.AddForce(pushOffWall * 1.0f);
  }

private void CheckIfWallSliding()
{
    if (touchingWall && !onGround && rb.velocity.y < 0 && moveInputDir != 0)
    {
        isWallSliding = true;
    }
    else if (!touchingWall)
    { 
        isWallSliding = false;
    }
}

private void Update()
{
    CheckInput();
    CheckMovementDir();
    CheckJump();
    CheckIfWallSliding();
}

private void FixedUpdate()
{
    onGround = collisions.GCollision();
    touchingWall = collisions.WCollision();
    ApplyMovement();
}

private void OnDrawGizmos()
{
   // Gizmos.DrawWireSphere(feet.position, 0.25f);
}
}
