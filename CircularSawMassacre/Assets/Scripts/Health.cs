using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(GameObject))]

public class Health : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    private bool gothit=false;
    BoxCollider2D mainCollider;
    public float invtime;
    public float invaktuell;
    public GameObject Player;
    public bool isAlive;
    public Renderer rend;

    void Start()
    {
        currentHP = maxHP;
        invaktuell = invtime;
        mainCollider = Player.GetComponent<BoxCollider2D>();
        isAlive = true;
    }

    void Update()
    {

        if (gothit == true)
        {
            invaktuell -= Time.deltaTime;
            if (invaktuell <= 0)
            {
                gothit = false;
                invaktuell = invtime;
            }
        }
        if (isAlive == false)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D Enemy)
    {
        if (Enemy.gameObject.CompareTag("Damage"))
        {
            ApplyDamage(-1);
            gothit = true;
        }
    }

    void ApplyDamage(int damageToApply)
    {
        
            currentHP += damageToApply;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            if (currentHP <= 0)
            {
                isAlive = false;
                FindObjectOfType<AudioManager>().Play("GameOver");
                Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
        
    }
}
