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


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");


        if (input.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        var acceleration = accel;
        var xVelocity = 0f;

        if(input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rigidbody.velocity.x;
        }

        rigidbody.AddForce(new Vector2(((input.x * speed) - rigidbody.velocity.x) * acceleration, 0));
        rigidbody.velocity = new Vector2(xVelocity, rigidbody.velocity.y);
    }
}
