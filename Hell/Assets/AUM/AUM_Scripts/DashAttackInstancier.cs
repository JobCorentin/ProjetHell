using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackInstancier : MonoBehaviour
{
    public static DashAttackInstancier dai;

    public GameObject slash;

    public float movementForce;

    public float coolDown;
    float coolDownTimer;

    public float duration;

    bool canDash = true;

    void Start()
    {
        dai = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(MovementController.mC.isGrounded)
        {
            canDash = true;
        }

        if (coolDownTimer < coolDown)
        {
            coolDownTimer += Time.fixedDeltaTime;
        }

        if (InputListener.iL.dashAttackInput == true && coolDownTimer >= coolDown && canDash == true)
        {
            //slash.SetActive(true);

            StartCoroutine(DashSlash());

            coolDownTimer = 0;

            canDash = false;
        }

        InputListener.iL.dashAttackInput = false;
    }

    IEnumerator DashSlash()
    {
        MovementController.mC.stuned = true;
        MovementController.mC.projected = false;
        slash.SetActive(true);

        MovementController.mC.rb.velocity = Vector2.zero;

        Vector2 attackDirection = InputListener.iL.directionVector.normalized;

        float attackDirectionAngle = Vector2.Angle(transform.right, attackDirection);

        if (attackDirection.y < 0)
        {
            attackDirectionAngle = -attackDirectionAngle;
        }

        slash.transform.rotation = Quaternion.Euler(0, 0, attackDirectionAngle);

        for (float i = duration; i >= 0; i -= Time.fixedDeltaTime)
        {
            MovementController.mC.rb.velocity = attackDirection * movementForce * i * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        slash.SetActive(false);
        MovementController.mC.stuned = false;
    }
}
