using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragBehaviour : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        float randomTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(randomTime);
        animator.SetTrigger("Fade");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
