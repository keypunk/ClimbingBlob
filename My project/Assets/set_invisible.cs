using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_invisible : MonoBehaviour
{
    public GameObject Player;
    public GameObject Deathscreen;
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        
    }

    void Update()
    {
        if ( Player.GetComponent<Health>().isAlive == false)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}