using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public GameObject Player;
    public GameObject sign;
    public Renderer rend;

    void Start()
    {
        sign.GetComponent<Renderer>().enabled = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sign.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sign.GetComponent<Renderer>().enabled = false;
        }
    }
}
