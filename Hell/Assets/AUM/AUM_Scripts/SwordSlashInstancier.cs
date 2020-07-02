using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlashInstancier : MonoBehaviour
{
    public static SwordSlashInstancier ssi;

    public GameObject slash;

    public float movementForce;

    public float coolDown;
    float coolDownTimer;

    public float duration;

    public float momentumMultiplier;

    Coroutine lastSlash;
    Coroutine lastStarting;

    void Start()
    {
        ssi = this;

        slash.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(coolDownTimer < coolDown)
        {
            coolDownTimer += Time.fixedDeltaTime;
        }

        if (InputListener.iL.attackInput == true && coolDownTimer > coolDown/2f)
        {
            AttackDirectionDecision();
        }

        InputListener.iL.attackInput = false;
    }

    void AttackDirectionDecision()
    {
        Vector2 currentAttackDirection = InputListener.iL.directionVector.normalized;
        Vector2 currentInputDirection = InputListener.iL.directionVector.normalized;

        float attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        if (MovementController.mC.isGrounded == true)
        {
            if (attackDirectionAngle < 45 && -90 < attackDirectionAngle)
                currentAttackDirection = Vector2.right;
            else if (45 <= attackDirectionAngle && attackDirectionAngle <= 135)
                currentAttackDirection = Vector2.up;
            else if (attackDirectionAngle < 135 || -90 < attackDirectionAngle)
                currentAttackDirection = Vector2.left;
        }
        else
        {
            if (attackDirectionAngle < 70 && -70 < attackDirectionAngle)
                currentAttackDirection = Vector2.right;
            else if (70 <= attackDirectionAngle && attackDirectionAngle <= 110)
                currentAttackDirection = Vector2.up;
            else if (attackDirectionAngle > 110 || -110 > attackDirectionAngle)
                currentAttackDirection = Vector2.left;
            else if (-110 <= attackDirectionAngle && attackDirectionAngle <= -70)
                currentAttackDirection = Vector2.down;
        }


        if (lastStarting == null)
            lastStarting = StartCoroutine(StartSlash(currentAttackDirection, currentInputDirection));
    }

    IEnumerator StartSlash(Vector2 currentAttackDirection, Vector2 currentInputDirection)
    {
        while (lastSlash != null || coolDownTimer < coolDown)
        {
            yield return null;
        }

        lastSlash = StartCoroutine(Slash(currentAttackDirection, currentInputDirection));

        lastStarting = null;
    }

    IEnumerator Slash(Vector2 currentAttackDirection, Vector2 currentInputDirection)
    {
        //MovementController.mC.stuned = true;
        MovementController.mC.projected = false;
        slash.SetActive(true);

        MovementController.mC.rb.velocity = Vector2.zero;
        MovementController.mC.rb.AddForce(currentInputDirection * movementForce * Time.fixedDeltaTime);

        float attackDirectionAngle = Vector2.Angle(transform.right, currentAttackDirection);

        if (currentAttackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        slash.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);

        BetterJump.bj.fallMultiplier /= 4;

        for (float i = duration + momentumMultiplier; i >= momentumMultiplier; i -= Time.fixedDeltaTime)
        {
            MovementController.mC.rb.velocity = currentInputDirection * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        BetterJump.bj.fallMultiplier *= 4;

        slash.transform.localScale = new Vector2(1, 1);

        slash.SetActive(false);
        MovementController.mC.stuned = false;

        coolDownTimer = 0;
        lastSlash = null;
    }
}
