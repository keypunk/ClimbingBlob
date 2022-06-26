using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelTrigger : MonoBehaviour
{
    private GameMaster gm;
    public int leveltoload;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            gm.lastCheckPointPos = Vector2.zero;
            SceneManager.LoadScene(leveltoload); 
        }
    }
}
