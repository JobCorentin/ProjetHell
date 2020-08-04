using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBehaviour : MonoBehaviour
{
    public int health;
    Animator animator;
    public bool isDestroyed;
    public List<GameObject> fragList;
    public GameObject fog;
    Transform t;
    Transform originalTransform;

    float timeBtweenDamage = 0.3f;

    bool IsHeal;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();

    }

    private void Start()
    {
        t = gameObject.transform.GetChild(0).transform;
        originalTransform = t;
    }

    public IEnumerator TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0 && isDestroyed == false)
        {
            FXManager.fxm.fxInstancier(3, originalTransform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));
            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.045f, 0f));
            CameraShake.cs.WeakShake();
            Destroyed();
        }
        else if (health > 0)
        {
            FXManager.fxm.fxInstancier(3, originalTransform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));
            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.045f, 0f));
            animator.SetTrigger("isHit");
            CameraShake.cs.WeakShake();
            Debug.Log(health + "Hp remaining");
        }

        yield return new WaitForSeconds(timeBtweenDamage);
    }

    public void ShakeCam()
    {
        CameraShake.cs.WeakShake();
    }

    void Destroyed()
    {

        SpawnFrag();
        SpawnFog();
        isDestroyed = true;
        animator.SetBool("isDestroyed", true);
    }

    void SpawnFrag()
    {   
        for (int i = 0; i < fragList.Count; i++)
        {
            t.position = gameObject.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject currentFrag = Instantiate(fragList[i], t);
            currentFrag.transform.Rotate(new Vector3(0, 0, Random.Range(45f, 270f)));
            currentFrag.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5, 5) * 200, Random.Range(-5, 5) * 200));
        }
    }

    void SpawnFog()
    {
        Instantiate(fog, originalTransform);
    }
}
