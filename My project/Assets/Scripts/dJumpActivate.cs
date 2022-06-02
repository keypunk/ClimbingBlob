using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class dJumpActivate : MonoBehaviour
{
    private enum animationStates { idle, activated}

    [SerializeField] private bool isForDJump;
    [SerializeField] private bool isForDash;
    public GameObject Player;
    private Animator anim;

    private bool activate;
    [SerializeField] private float cdTime;
    private float timer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activate)
        {
            if (isForDJump)
            {
                Player.GetComponent<PlayerMovement>().dJumpReady = true;
            }
            if (isForDash)
            {
                Player.GetComponent<PlayerMovement>().dashReady = true;
            }
            activate = true;
        }
    }
    private void Update()
    {
        if (activate)
        {
            activate = false;
            timer -= Time.deltaTime;
            if ( timer <= 0)
            {
                activate = false;
                timer = cdTime;
            }
        }
        animationStates state;
        if (activate)
        {
            state = animationStates.activated;
        }
        else
        {
            state = animationStates.idle;
        }
        anim.SetInteger("state", (int)state);
    }

    private void UpdateAnimationState()
    {
        animationStates state;
        if (activate)
        {
            state = animationStates.activated;
        }
        else
        {
            state = animationStates.idle;
        }
        anim.SetInteger("state", (int)state);
    }
}
