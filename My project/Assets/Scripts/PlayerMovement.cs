using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    public ParticleSystem dJumpParticles;
    public ParticleSystem dashParticles;

    public bool dJumpReady;
    public bool dJump;
    public bool middJump;

    private float dashTimeCounter;
    public bool dashReady;
    private bool midDash;
    private bool dashRight;

    public float dirX;


    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float djumpForce = 13f;
    [SerializeField] private float dashForce = 13f;
    [SerializeField] private float dashTime = 13f;



    private enum MovementState { idle, running, jumping, falling, dJumping, dashing}

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        dJumpParticles.Stop();
        dashParticles.Stop();
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
            dashReady = false;
            dJumpParticles.Stop();
            dashParticles.Stop();
        }

        if (dJumpReady)
        {
            dJumpParticles.Play();
        }

        if (dashReady)
        {
            dashParticles.Play();
        }
        // aktiviert dJump
        if (Input.GetButtonDown("Jump") && dJumpReady && !isGrounded() && !midDash)
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            middJump = true;
            rb.velocity = new Vector2(rb.velocity.x, djumpForce);
            dJumpReady = false;
            dJumpParticles.Stop();

        }
        
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            FindObjectOfType<AudioManager>().Play("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            dJumpReady = false;
            dJumpParticles.Stop();
        }

        if (Input.GetButton("Jump") && isJumping && !midDash)
        {
            if(jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping=false;
            }
        }
        if (Input.GetButton("Fire3") && dashReady)
        {
            if (dirX > 0)
            {
                rb.velocity = new Vector2(dashForce, 0f);
                dashRight = true;
            }
            else if (dirX < 0)
            {
                rb.velocity = new Vector2(-dashForce, 0f);
                dashRight = false;
            }
            dashReady = false;
            dashParticles.Stop();
            midDash = true;
            dashTimeCounter -= Time.deltaTime;
        }
        if (midDash && dashTimeCounter > 0)
        {
            if (dashRight)
            {
                rb.velocity = new Vector2(dashForce, 0f);
            }
            else if (!dashRight)
            {
                rb.velocity = new Vector2(-dashForce, 0f);
            }
            dashTimeCounter -= Time.deltaTime;
        }
        else if (!isGrounded())
        {
            dashTimeCounter = dashTime;
            midDash = false;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
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
        if (dirX > 0f && !midDash)
        {
            state = MovementState.running;
            sprite.flipX = false; 
        }
        else if (dirX < 0f && !midDash)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (midDash && dashRight)
        {
            state = MovementState.dashing;
            sprite.flipX = false;
        }
        else if (midDash && !dashRight)
        {
            state = MovementState.dashing;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && middJump && !midDash)
        {
            state = MovementState.dJumping;
        }
        else if (rb.velocity.y > .1f && !midDash)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f && !midDash)
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