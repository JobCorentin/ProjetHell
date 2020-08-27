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
    public GameObject wheel;
    Transform t;
    Transform originalTransform;

    float timeBtweenDamage = 0.3f;

    bool IsHeal;
    public bool isChar;



    [Space(10)]
    [Header("Sounds")]
    public AK.Wwise.Event woodBreakAudio;


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

            Healthpack hp = GetComponent<Healthpack>();

            if (hp != null)
            {
                if (HealthManager.hm.life != HealthManager.hm.initialLife)
                {
                    Destroyed();

                    hp.HealPlayer();
                }
            }
            else
            {
                Destroyed();
            }
        }
        else if (health > 0)
        {
            FXManager.fxm.fxInstancier(3, originalTransform, BaseSlashInstancier.bsi.attackDirectionAngle + Random.Range(-10, 10));
            FreezTimeManager.ftm.StartCoroutine(FreezTimeManager.ftm.FreezeTimeFor(0.045f, 0f));
            animator.SetTrigger("isHit");
            CameraShake.cs.WeakShake();
        }

        yield return new WaitForSeconds(timeBtweenDamage);
    }

    public void ShakeCam()
    {
        CameraShake.cs.WeakShake();
    }

    void Destroyed()
    {
        woodBreakAudio.Post(gameObject);


        if(isChar== true)
        {
            SpawnWheel();
        }
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
            currentFrag.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3, 3) * 10, Random.Range(-3, 3) * 10),ForceMode2D.Impulse);
        }
    }

    void SpawnWheel()
    {
        GameObject currentWheel = Instantiate(wheel, originalTransform);
        currentWheel.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3, 3) * 10, Random.Range(-3, 3) * 10),ForceMode2D.Impulse);
    }

    void SpawnFog()
    {
        Instantiate(fog, originalTransform);
    }
}
