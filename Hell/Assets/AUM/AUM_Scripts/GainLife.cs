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

        if (InputListener.iL.parryInput == true && cooldownTimer >= cooldown)
        {
            if (BloodManager.bm.bloodNumb >= 6)
                StartCoroutine(UseBlood());
        }

        InputListener.iL.parryInput = false;
    }

    IEnumerator UseBlood()
    {
        MovementController.mC.stuned = true;

        if(BloodManager.bm.bloodNumb < BloodManager.bm.bloodNumbMax)
        {
            if (HealthManager.hm.life < HealthManager.hm.initialLife)
                HealthManager.hm.life += 1;
        }
        else if (BloodManager.bm.bloodNumb == BloodManager.bm.bloodNumbMax)
        {
            HealthManager.hm.life = HealthManager.hm.initialLife;
        }

        BloodManager.bm.bloodNumb = 0;

        for (float i = recoveryDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        MovementController.mC.stuned = false;

        cooldownTimer = 0;

        yield break;

    }
}
