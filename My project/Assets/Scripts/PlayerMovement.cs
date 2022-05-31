using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public bool dJumpReady;
    public bool dJump;
    public bool middJump;
    [SerializeField] private LayerMask jumpableGround;
    
    private float dirX;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float djumpForce = 13f;


    private enum MovementState { idle, running, jumping, falling, dJumping}

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // setzt dublejump auf aus
        if (isGrounded())
        {
            middJump = false;
            dJump = false;
            dJumpReady = false;
        }

        // aktiviert dJump
        if (Input.GetButtonDown("Jump") && dJumpReady && !isGrounded())
        {
            middJump = true;
            rb.velocity = new Vector2(rb.velocity.x, djumpForce);
            dJumpReady = false;
        }
        
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            dJumpReady = false;
        }
        if (!Input.GetButtonDown("Jump") && !isGrounded())
        {
            dJump = true;
        }
        UpdateAnimationState();
        
    }
    
     private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false; 
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && middJump)
        {
            state = MovementState.dJumping;
        }
        else if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}