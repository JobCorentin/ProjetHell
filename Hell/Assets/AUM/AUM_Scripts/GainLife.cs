using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainLife : MonoBehaviour
{

    public static GainLife gl;

    public float recoveryDuration;

    public float cooldown;
    float cooldownTimer = 0;

    public float attackSpeed;

    Coroutine lastParry;

    public GameObject boomerangPrefab;

    [HideInInspector] public bool noSword = false;

    float inputPressedFor;

    bool gainingLife = false;

    bool wasPressed;

    BoomController currentBoomerang;

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

        if (inputPressedFor >= 0.5f && MovementController.mC.isGrounded && gainingLife == false)
        {
            if (BloodManager.bm.bloodNumb >= 3)
                StartCoroutine(UseBlood());
        }

        if (inputPressedFor < 0.5f && wasPressed == true && InputListener.iL.parryInput == false && noSword == true)
        {
            StartCoroutine(DashToBoomerang());
        }

        if (inputPressedFor < 0.5f && wasPressed == true && InputListener.iL.parryInput == false && noSword == false)
        {
            if (BloodManager.bm.bloodNumb >= 3)
                LaunchBoomerang();
        }

        if (InputListener.iL.parryInput)
        {
            inputPressedFor += Time.fixedDeltaTime;
        }
        else
        {
            inputPressedFor = 0;
        }

        wasPressed = InputListener.iL.parryInput;
    }

    IEnumerator UseBlood()
    {
        cooldownTimer = 0;

        gainingLife = true;

        MovementController.mC.stuned = true;

        MovementController.mC.rb.velocity = Vector2.zero;

        int bloodNumbMinus3 = BloodManager.bm.bloodNumb - 3;

        while (BloodManager.bm.bloodNumb > bloodNumbMinus3)
        {
            BloodManager.bm.bloodNumb -= 1;

            yield return new WaitForSeconds(0.5f);
        }

        /*for (float i = recoveryDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }*/

        MovementController.mC.stuned = false;

        if (HealthManager.hm.life < HealthManager.hm.initialLife)
            HealthManager.hm.life += 1;

        gainingLife = false;

        yield break;

    }

    void LaunchBoomerang()
    {
        MovementController.mC.animator.SetTrigger("Throwing");

        BloodManager.bm.bloodNumb -= 3;

        currentBoomerang = Instantiate(boomerangPrefab).GetComponent<BoomController>();

        currentBoomerang.transform.position = transform.position;

        currentBoomerang.target = (Vector2)transform.position + ((InputListener.iL.directionVector).normalized * 15f);

        currentBoomerang.speed = attackSpeed;

        StartCoroutine(currentBoomerang.GoToTargetThenPlayer());
    }

    IEnumerator DashToBoomerang()
    {
        MovementController.mC.col.enabled = false;

        while (Vector2.Distance(transform.position, currentBoomerang.transform.position) > 3f)
        {
            MovementController.mC.rb.velocity = (currentBoomerang.transform.position - transform.position).normalized * 60f;

            yield return null;
        }

        MovementController.mC.rb.AddForce((currentBoomerang.transform.position - transform.position).normalized, ForceMode2D.Impulse);

        currentBoomerang.StopAllCoroutines();

        Destroy(currentBoomerang.gameObject);

        noSword = false;

        MovementController.mC.col.enabled = true;

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
