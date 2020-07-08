using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public static Parry p;

    public Transform protectionCol;

    public float protectionDuration;
    public float recoveryDuration;

    public float cooldown;
    float cooldownTimer = 0;

    Coroutine lastParry;

    public bool parrying;

    // Start is called before the first frame update
    void Start()
    {
        p = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(cooldownTimer < cooldown)
        {
            cooldownTimer += Time.fixedDeltaTime;
        }

        if(InputListener.iL.parryInput == true && parrying == false && cooldownTimer >= cooldown)
        {
            lastParry = StartCoroutine(ActivateParry());
        }

        InputListener.iL.parryInput = false;
    }

    IEnumerator ActivateParry()
    {
        parrying = true;

        protectionCol.gameObject.SetActive(true);

        Vector2 currentParryDirection = InputListener.iL.directionVector.normalized;

        float parryDirectionAngle = Vector2.Angle(transform.right, currentParryDirection);

        if (currentParryDirection.y < 0)
        {
            parryDirectionAngle = -parryDirectionAngle;
        }

        if (MovementController.mC.isGrounded == true)
        {
            if (parryDirectionAngle < 45 && -90 < parryDirectionAngle)
                parryDirectionAngle = 0;
            else if (45 <= parryDirectionAngle && parryDirectionAngle <= 135)
                parryDirectionAngle = 90;
            else if (parryDirectionAngle < 135 || -90 < parryDirectionAngle)
                parryDirectionAngle = 180;
        }
        else
        {
            if (parryDirectionAngle < 45 && -45 < parryDirectionAngle)
                parryDirectionAngle = 0;
            else if (45 <= parryDirectionAngle && parryDirectionAngle <= 135)
                parryDirectionAngle = 90;
            else if (parryDirectionAngle > 135 || -135 > parryDirectionAngle)
                parryDirectionAngle = 180;
            else if (-135 <= parryDirectionAngle && parryDirectionAngle <= -45)
                parryDirectionAngle = -90;
        }

        protectionCol.transform.rotation = Quaternion.Euler(0, 0, parryDirectionAngle);

        BetterJump.bj.StopLastChangeFall();

        BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine(BetterJump.bj.ChangeFallMultiplier(protectionDuration + recoveryDuration, BetterJump.bj.fallMultiplier / 10f));


        MovementController.mC.StopLastChangeSpeed();

        MovementController.mC.lastChangeSpeed = MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(protectionDuration + recoveryDuration, MovementController.mC.speed / 5f));

        for (float i = protectionDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        protectionCol.gameObject.SetActive(false);

        for (float i = recoveryDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        cooldownTimer = 0;

        parrying = false;

        yield break;

    }

    public void StopParry()
    {
        BetterJump.bj.StopLastChangeFall();

        MovementController.mC.StopLastChangeSpeed();

        //protectionCol.gameObject.SetActive(false);

        //cooldownTimer = 0;

        //parrying = false;

        /*if(lastParry != null)
            StopCoroutine(lastParry);*/
    }
}
