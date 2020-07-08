using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnnemiController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Seeker seeker;

    [HideInInspector] public Vector2 direction;

    public bool tough;
    public int health;

    public float speed;

    public int type;

    public SpriteRenderer sr;

    [HideInInspector] public bool stunned;

    public Transform pTransform;
    [HideInInspector] public Vector2 target;

    public float range;

    public float nextWayPointDistance;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndPoint = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if(path!= null)
        {
            if(currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndPoint = true;
                return;
            }
            else
            {
                reachedEndPoint = false;
            }

            direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

            rb.AddForce(direction * speed * Time.fixedDeltaTime);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

            if(distance < nextWayPointDistance)
            {
                currentWayPoint++;
            }
        }
    }

    public IEnumerator TakeDamage(int amount)
    {

        if(tough == true)
        {
            if (health <= 0)
            {
                StartCoroutine( Execute() );
            }
            else
            {
                health -= amount;
                if (amount == 1)
                {
                    CameraShake.cs.WeakShake();
                }
                else if (amount == 2)
                {
                    CameraShake.cs.StrongShake();
                }
                
            }
        }
        

        sr.color = Color.red;

        yield return new WaitForSeconds(0.3f * amount);

        sr.color = Color.white;

        if(tough == false)
        {
            Die();
        }

    }

    public IEnumerator Execute()
    {
        /*Time.timeScale = 1f;

        while (Time.timeScale > 0.5f)
        {
            if (Time.timeScale - 100f * Time.unscaledDeltaTime >= 0.5f)
            {
                Time.timeScale = Time.timeScale - 100f * Time.unscaledDeltaTime;
            }
            else
            {
                Time.timeScale = 0.5f;
            }

            yield return null;
        }

        Time.timeScale = 0.5f;

        yield return new WaitForSecondsRealtime(1f);

        while (Time.timeScale < 1f)
        {
            if (Time.timeScale - 100f * Time.unscaledDeltaTime <= 1f)
            {
                Time.timeScale = Time.timeScale + 100f * Time.unscaledDeltaTime;
            }
            else
            {
                Time.timeScale = 1f;
            }
            yield return null;
        }

        Time.timeScale = 1f;*/

        /*
        for (float i = 1f; i >= 0f; i -= (0.02f * 4f))
        {
            Time.timeScale = i;

            sr.color = Color.red;
            yield return new WaitForSecondsRealtime(0.02f);
            sr.color = Color.red;
        }

        yield return new WaitForSecondsRealtime(0.25f);

        for (float i = 0f; i <= 1f; i += (0.02f * 4f))
        {
            Time.timeScale = i;

            sr.color = Color.red;
            yield return new WaitForSecondsRealtime(0.02f);
            sr.color = Color.red;
        }

        Time.timeScale = 1f;
        */

        Die();

        yield break;
    }

    public void Die()
    {
        MovementController.mC.canDoubleJump = true;

        gameObject.SetActive(false);
    }
}
