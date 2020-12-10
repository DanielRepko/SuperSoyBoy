using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class SoyBoyController : MonoBehaviour
{
    public float speed = 14f;
    public float accel = 6f;
    private Vector2 input;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private Animator animator;

    //used to determine if the player is jumping
    public bool isJumping;
    public float jumpSpeed = 8f;
    //sets the lenght of the raycast used to check if the player is on the ground
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;

    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration;
    public float jump = 14f;

    public float airAccel = 3f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool PlayerIsOnGround()
    {
        bool groundCheck1 = Physics2D.Raycast(new Vector2(
        transform.position.x, transform.position.y - height),
        -Vector2.up, rayCastLengthCheck);

        bool groundCheck2 = Physics2D.Raycast(new Vector2(
        transform.position.x + (width - 0.2f),
        transform.position.y - height), -Vector2.up,
        rayCastLengthCheck);

        bool groundCheck3 = Physics2D.Raycast(new Vector2(
        transform.position.x - (width - 0.2f),
        transform.position.y - height), -Vector2.up,
        rayCastLengthCheck);

        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAgainstWall()
    {
        bool wallOnLeft = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), -Vector2.right, rayCastLengthCheck);
        bool wallOnRight = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y), Vector2.right, rayCastLengthCheck);

        if(wallOnLeft || wallOnRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerIsTouchingGroundOrWall()
    {
        if (PlayerIsOnGround() || IsAgainstWall())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWallDirection()
    {
        bool wallOnLeft = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), -Vector2.right, rayCastLengthCheck);
        bool wallOnRight = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y), Vector2.right, rayCastLengthCheck);

        if (wallOnLeft)
        {
            return -1;
        }
        else if (wallOnRight)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");

        animator.SetFloat("Speed", Mathf.Abs(input.x));

        //flips the sprite to change which direction the player is facing
        if (input.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < 0f)
        {
            spriteRenderer.flipX = true;
        }

        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;
            animator.SetBool("IsJumping", true);
        }
        else
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
            jumpDuration = 0f;
        }

        //allows the player to jump only if they are on the ground and not already jumping
        if (PlayerIsOnGround() && isJumping == false)
        {
            if(input.y > 0f)
            {
                isJumping = true;
            }

            animator.SetBool("IsOnWall", false);
        }


        if (jumpDuration > jumpDurationThreshold) input.y = 0f;
    }

    private void FixedUpdate()
    {
        var acceleration = accel;
        var xVelocity = 0f;

        if(PlayerIsOnGround() && input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rigidbody.velocity.x;
        }
        var yVelocity = 0f;
        if(PlayerIsTouchingGroundOrWall() && input.y == 1)
        {
            yVelocity = jump;
        }
        else
        {
            yVelocity = rigidbody.velocity.y;
        }

        rigidbody.AddForce(new Vector2(((input.x * speed) - rigidbody.velocity.x) * acceleration, 0));
        rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        if(IsAgainstWall() && !PlayerIsOnGround() && input.y == 1)
        {
            rigidbody.velocity = new Vector2(-GetWallDirection() * speed * 0.75f, rigidbody.velocity.y);
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);
        }
        else if (!IsAgainstWall())
        {
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);
        }

        if(IsAgainstWall() && !PlayerIsOnGround())
        {
            animator.SetBool("IsOnWall", true);
        }

        if(isJumping && jumpDuration < jumpDurationThreshold)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }
    }
}
