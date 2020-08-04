using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainLife : MonoBehaviour
{
    public static GainLife gl;

    public float recoveryDuration;

    public float cooldown;
    float cooldownTimer = 0;

    Coroutine lastParry;

    // Start is called before the first frame update
    void Start()
    {
        gl = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldownTimer < cooldown)
        {
            cooldownTimer += Time.fixedDeltaTime;
        }

        if (InputListener.iL.parryInput == true && cooldownTimer >= cooldown && MovementController.mC.isGrounded)
        {
            if (BloodManager.bm.bloodNumb >= 6)
                StartCoroutine(UseBlood());
        }

        InputListener.iL.parryInput = false;
    }

    IEnumerator UseBlood()
    {
        MovementController.mC.stuned = true;

        MovementController.mC.rb.velocity = Vector2.zero;

        bool gainEverything = false;

        if(BloodManager.bm.bloodNumb < BloodManager.bm.bloodNumbMax)
        {
            gainEverything = false;
        }
        else if (BloodManager.bm.bloodNumb == BloodManager.bm.bloodNumbMax)
        {
            gainEverything = true;
        }

        while(BloodManager.bm.bloodNumb > 0)
        {
            BloodManager.bm.bloodNumb -= 1;

            yield return new WaitForSeconds(0.2f);
        }

        /*for (float i = recoveryDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }*/

        MovementController.mC.stuned = false;

        if (!gainEverything)
        {
            if (HealthManager.hm.life < HealthManager.hm.initialLife)
                HealthManager.hm.life += 1;

            StartCoroutine(ChangeSpeedMultiplierFor(1, 8f));
        }
        else
        {
            HealthManager.hm.life = HealthManager.hm.initialLife;

            StartCoroutine(ChangeSpeedMultiplierFor(2, 16f));
        }

        cooldownTimer = 0;

        yield break;

    }

    IEnumerator ChangeSpeedMultiplierFor(float valueBonus, float time)
    {

        for(float i = time; i > 0; i -= Time.fixedDeltaTime)
        {
            float temp = i / time;

            MovementController.mC.speedMultiplier = (valueBonus * temp) + 1;

            yield return new WaitForEndOfFrame();
        }

        MovementController.mC.speedMultiplier = 1;
    }
}
