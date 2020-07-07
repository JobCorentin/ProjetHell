using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBehaviour : MonoBehaviour
{
    public int health;
    Animator animator;

    float timeBtweenDamage = 0.3f;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroyed();
        }
        else if (health > 0)
        {       
            animator.SetTrigger("isHit");
            Debug.Log(health + "Hp remaining");
        }

        yield return new WaitForSeconds(timeBtweenDamage);
    }

    void Destroyed()
    {
        Debug.Log(health + "Game over");
        animator.SetBool("isDestroyed", true);
    }
}
