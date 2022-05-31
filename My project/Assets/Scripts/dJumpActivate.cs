using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class dJumpActivate : MonoBehaviour
{
    [SerializeField] private bool isForDJump;
    [SerializeField] private bool isForDash;
    public GameObject Player;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isForDJump)
            {
                Player.GetComponent<PlayerMovement>().dJumpReady = true;
            }
            if (isForDash)
            {
                Player.GetComponent<PlayerMovement>().dashReady = true;
            }
        }
    }
}
