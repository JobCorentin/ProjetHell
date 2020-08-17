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
    public GameObject DashEffect; 

    // Start is called before the first frame update
    void Start()
    {
        gl = this;
        DashEffect.SetActive(false);
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
            BaseSlashInstancier.bsi.sr.color = new Color(BaseSlashInstancier.bsi.sr.color.r, BaseSlashInstancier.bsi.sr.color.g, BaseSlashInstancier.bsi.sr.color.b, (BloodManager.bm.bloodNumb - bloodNumbMinus3) / 3);

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

        yield return new WaitForSeconds(0.5f);


        BaseSlashInstancier.bsi.sr.color = new Color(BaseSlashInstancier.bsi.sr.color.r, BaseSlashInstancier.bsi.sr.color.g, BaseSlashInstancier.bsi.sr.color.b, 1);
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
        StartCoroutine(dashFx(0.2f, 0.3f));
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

    IEnumerator dashFx(float time1, float time2)
    {
        var main = DashEffect.GetComponent<ParticleSystem>();
        var main2 = DashEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();

        DashEffect.SetActive(true);
        if(MovementController.mC.rb.velocity.x < 0)
        {
            DashEffect.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (MovementController.mC.rb.velocity.x > 0)
        {
            DashEffect.transform.localScale = new Vector3(1, 1, 1);
        }
        yield return new WaitForSeconds(time1);
        main.loop = false;
        main2.loop = false;
        yield return new WaitForSeconds(time2);
        main.loop = true;
        main2.loop = true;
        DashEffect.SetActive(false);
    }
}
