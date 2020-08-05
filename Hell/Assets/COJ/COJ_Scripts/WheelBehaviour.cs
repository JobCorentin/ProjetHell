using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    bool isRolling;
    bool isMoveable;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isMoveable = true;
        isRolling = true;
        StartCoroutine(Rolling());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9 && isMoveable == true)
        {
            FXManager.fxm.fxInstancier(3, transform, 0);
            rb.AddForce(new Vector2((transform.position.x - collision.transform.position.x), 1f / 3f).normalized * 300);
            isRolling = true;
        }
    }

    IEnumerator Rolling()
    {
        yield return new WaitForSeconds(2f);
        if(isRolling == true)
        {
            isRolling = false;
            StartCoroutine(Rolling());
        }
        else if (isRolling == false)
        {
            isMoveable = false;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("isFalling");
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
