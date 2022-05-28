using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class MalteTest_CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;
    public GameObject Player;
    


    public bool Dash;
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    private int direction;
    private bool initialDash;

    private bool facingRight = true;
    float moveDirection = 0;
    public bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    bool dashUp;
    public bool getfacingRight()
    {
        return facingRight;
    }

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
        dashTime = startDashTime;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (Player.GetComponent<Health>().isAlive)
        {

        

            // Movement controls
            if (dashTime == startDashTime)
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) )
            {
                 moveDirection = Input.GetKey(KeyCode.LeftArrow) ? -1 : 1;
            }
            else
            {
                moveDirection = 0;
            }

            // Change facing direction
            
            
                    if (moveDirection > 0 && !facingRight)
                    {
                        facingRight = true;
                        //t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
                    }
                    if (moveDirection < 0 && facingRight)
                    {
                        facingRight = false;
                        //t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                    }
            
            }
        
        
            // Jumping
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {

                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }

            //  Dash
            if (dashUp)
            {
                initialDash = true;
            }
            if (Dash && direction == 0)
            {
                if (isGrounded)
                {
                    dashUp = true;
                }
                if (facingRight && Input.GetKeyDown(KeyCode.LeftShift) && dashUp)
                {
                    direction = 1;
                    dashUp = false;
                }
                else if (!facingRight && Input.GetKeyDown(KeyCode.LeftShift) && dashUp)
                {
                    direction = 2;
                    dashUp = false;
                }
            }
            else
            {
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                }
                else
                {
                    dashTime -= Time.deltaTime;

                    if (facingRight == true)
                    {
                        if (initialDash)
                        {
                            r2d.velocity = new Vector2(r2d.velocity.x + (moveDirection) * dashSpeed, 0f);
                            initialDash = false;
                        }
                        else
                        {
                            r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y);
                        }


                    }
                    else if (facingRight == false)
                    {
                        if (initialDash)
                        {
                            r2d.velocity = new Vector2(r2d.velocity.x + (moveDirection) * dashSpeed, 0f);
                            initialDash = false;
                        }
                        else
                        {
                            r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y);
                        }
                    }
                }
            }
        }

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        if (dashTime == startDashTime)
        {
            if (moveDirection != 0 && isGrounded && Mathf.Abs(r2d.velocity.x) <=maxSpeed+1f)
            {
                r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
            }
            else if (isGrounded && Mathf.Abs(r2d.velocity.x) >1f && moveDirection == 0)
            {
                r2d.velocity = new Vector2(r2d.velocity.x * 0.9f, r2d.velocity.y);
            }

            else if (Mathf.Abs(r2d.velocity.x) < maxSpeed && moveDirection != 0 && !isGrounded)
            {
                r2d.velocity = new Vector2(maxSpeed * moveDirection, r2d.velocity.y);
            }

            else if (Mathf.Abs(r2d.velocity.x) > maxSpeed && (moveDirection > 0 && r2d.velocity.x > 0 || moveDirection < 0 && r2d.velocity.x < 0 ) && !isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x + 0.1f * moveDirection, r2d.velocity.y);
            }

            else if (moveDirection > 0 && r2d.velocity.x < 0 && !isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x * -0.1f, r2d.velocity.y);
            }

            else if (moveDirection < 0 && r2d.velocity.x > 0 && !isGrounded)
            {
                r2d.velocity = new Vector2(0f, r2d.velocity.y);
            }

            else if (moveDirection == 0 && Mathf.Abs(r2d.velocity.x) > 0 && !isGrounded)
            {
                r2d.velocity = new Vector2(r2d.velocity.x * 0.97f, r2d.velocity.y);
            }
        }

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}