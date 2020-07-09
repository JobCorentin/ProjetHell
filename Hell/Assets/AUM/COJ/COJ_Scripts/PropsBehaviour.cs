using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBehaviour : MonoBehaviour
{
    public int health;
    Animator animator;
    bool isDestroyed;

    float timeBtweenDamage = 0.3f;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public IEnumerator TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0 && isDestroyed == false)
        {
            CameraShake.cs.PropShake();
            Destroyed();
        }
        else if (health > 0)
        {       
            animator.SetTrigger("isHit");
            CameraShake.cs.PropShake();
            Debug.Log(health + "Hp remaining");
        }

        yield return new WaitForSeconds(timeBtweenDamage);
    }

    void Destroyed()
    {
        isDestroyed = true;
        Debug.Log(health + "Game over");
        animator.SetBool("isDestroyed", true);
    }
}
