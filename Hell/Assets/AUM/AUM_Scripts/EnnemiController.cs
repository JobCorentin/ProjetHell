using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class EnnemiController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Seeker seeker;
    public Animator animator;

    public bool playerDetected;

    public float detectionDistance;
    public LayerMask detectionLayers;

    [HideInInspector] public Vector2 direction;

    public bool tough;
    public int health;

    public float speed;

    public int type;

    public SpriteRenderer sr;

    public Collider2D col;

    Material defautlMaterial;
    public Material shaderMaterial1;
    public Material shaderMaterial2;

    public int numberBetweenGroupAttack;
    public float timeBetweenGroupAttack;

    [HideInInspector] public bool stunned;

    [HideInInspector] public bool hasAttacked = false;
    [HideInInspector] public int numbWhoHasAttacked = 0;

    public Transform pTransform;
    [HideInInspector] public Vector2 target;

    public float range;

    public float nextWayPointDistance;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndPoint = false;

    [HideInInspector] public EnnemiController[] ennemy_Controllers = new EnnemiController[] { };

    public float coolDown;
    [HideInInspector] public float coolDownTimer = 0;

    public bool isFlying;

    public bool playerWasDetected;

    public GameObject Remains;

    public bool hasSpawn;

    public bool dead = false;
    [HideInInspector] public Vector2 initialPosition;
    [HideInInspector] public int initialHealth;

    // Start is called before the first frame update
    public virtual void Start()
    {
        dead = false;

        initialPosition = transform.position;
        initialHealth = health;

        if(numberBetweenGroupAttack == 0)
        {
            numberBetweenGroupAttack = 1;
        }

        if (timeBetweenGroupAttack == 0)
        {
            timeBetweenGroupAttack = 0.6f;
        }

        pTransform = MovementController.mC.transform;
        defautlMaterial = gameObject.GetComponent<SpriteRenderer>().material;

        InvokeRepeating("GetEnnemies", 1f, 1f);
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
        

        if (hasSpawn == true)
        {

            if (playerWasDetected == false && playerDetected == true)
            {
                InvokeRepeating("UpdatePath", 0f, 0.3f);
            }

            if (path != null)
            {
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    reachedEndPoint = true;
                    return;
                }
                else
                {
                    reachedEndPoint = false;
                }

                direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                if (isFlying)
                {
                    rb.AddForce(direction * speed * Time.fixedDeltaTime);
                }
                else
                {

                    rb.AddForce(new Vector2(direction.x, 0) * speed * Time.fixedDeltaTime);
                }

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

                if (distance < nextWayPointDistance)
                {
                    currentWayPoint++;
                }
            }

            playerWasDetected = playerDetected;

        }
    }

    void GetEnnemies()
    {
        ennemy_Controllers = transform.parent.GetComponentsInChildren<EnnemiController>();

        for(int i = 0; i < ennemy_Controllers.Length; i++)
        {
            for(int x = 0; i < ennemy_Controllers.Length; i++)
            {
                if(Vector2.Distance(MovementController.mC.rb.transform.position, ennemy_Controllers[i].transform.position) > 
                    Vector2.Distance(MovementController.mC.rb.transform.position, ennemy_Controllers[x].transform.position) && i < x)
                {
                    EnnemiController temp = ennemy_Controllers[i];
                    ennemy_Controllers[i] = ennemy_Controllers[x];
                    ennemy_Controllers[x] = temp;
                }
            }
        }
    }

    public void Detection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MovementController.mC.transform.position - transform.position, detectionDistance, detectionLayers);

        if(hit == true)
            if(hit.collider.tag == "Player")
            {
                foreach (EnnemiController ennemy_Controller in ennemy_Controllers)
                {
                    ennemy_Controller.playerDetected = true;
                }

                playerDetected = true;
            }


    }

    public IEnumerator TakeDamage(int amount)
    {
        if (hasSpawn == true)
        {

            MovementController.mC.canDoubleJump = true;
            BaseSlashInstancier.bsi.slashNumb = BaseSlashInstancier.bsi.slashNumbMax;
            BaseSlashInstancier.bsi.canGainHeight = true;

            if (amount == 1)
            {
                CameraShake.cs.WeakShake();
            }
            else if (amount == 2)
            {
                CameraShake.cs.StrongShake();
            }

            health -= amount;
            if (health > 0)
            {
                //animator.SetTrigger("IsTakingDamage");

                sr.material = shaderMaterial1;

                yield return new WaitForSeconds(0.1f);

                sr.material = shaderMaterial2;

                yield return new WaitForSeconds(0.05f);

                sr.material = defautlMaterial;
            }
            else if (health <= 0)
            {
                Die();
            }

            /*if (tough == false)
            {
                Die();
            }*/
        }
    }

    public IEnumerator DamageDash(Vector2 dashDirection, float duration, float movementForce, float momentumMultiplier)
    {
        stunned = true;

        Vector2 currentAttackDirection = dashDirection.normalized;

        float attackDirectionAngle = Vector2.Angle(Vector2.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        if (attackDirectionAngle < 70 && -70 < attackDirectionAngle)
            currentAttackDirection = Vector2.right;
        else if (70 <= attackDirectionAngle && attackDirectionAngle <= 110)
            currentAttackDirection = Vector2.up;
        else if (attackDirectionAngle > 110 || -110 > attackDirectionAngle)
            currentAttackDirection = Vector2.left;
        else if (-110 <= attackDirectionAngle && attackDirectionAngle <= -70)
            currentAttackDirection = Vector2.down;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            rb.velocity = (currentAttackDirection.normalized * 1.5f + dashDirection.normalized).normalized * movementForce * 1.3f * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        stunned = false;
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

        animator.SetTrigger("Dying");


        //gameObject.SetActive(false);
    }

    public IEnumerator HasAttackedFor(float time)
    {
        hasAttacked = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        hasAttacked = false;
    }

    public void CheckingIfAlive()
    {
        if (HealthManager.hm.lastCheckPoint != null)
            if (HealthManager.hm.lastCheckPoint.respawning == true)
            {
                transform.position = initialPosition;
                health = initialHealth;


                sr.enabled = true;

                playerDetected = false;

                animator.SetBool("Respawning", true);

                dead = false;
            }
            else
            {
                animator.SetBool("Respawning", false);
            }

        if (dead == true)
        {
            playerDetected = false;
            sr.enabled = false;

            return;
        }

        if (dead == false)
        {
            sr.enabled = true;
        }
    }

    public void spawnRemains()
    {
        GameObject newRemains = Instantiate(Remains, gameObject.transform);
        newRemains.transform.parent = GameObject.Find("RemainHolder").transform;
        dead = true;
    }

    public void deactivate()
    {
        if(type == 0)
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(false);
    }

    public void spawning()
    {
        hasSpawn = false;
    }

    public void spawned()
    {
        hasSpawn = true;
    }
}
