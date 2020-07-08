using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public Collider2D normalCol;
    public Collider2D protectionCol;

    public float protectionDuration;
    public float recoveryDuration;

    public float cooldown;
    float cooldownTimer = 0;

    public bool parrying;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(InputListener.iL.parryInput == true)
        {
            StartCoroutine(ActivateParry());
        }

        InputListener.iL.parryInput = false;
    }

    IEnumerator ActivateParry()
    {
        normalCol.enabled = false;
        protectionCol.gameObject.SetActive(true);

        BetterJump.bj.StopLastChangeFall();

        BetterJump.bj.lastChangeFall = BetterJump.bj.StartCoroutine(BetterJump.bj.ChangeFallMultiplier(protectionDuration + recoveryDuration, BetterJump.bj.fallMultiplier / 10f));


        MovementController.mC.StopLastChangeSpeed();

        MovementController.mC.lastChangeSpeed = MovementController.mC.StartCoroutine(MovementController.mC.ChangeSpeed(protectionDuration + recoveryDuration, MovementController.mC.speed / 5f));

        for (float i = protectionDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        normalCol.enabled = true;
        protectionCol.gameObject.SetActive(false);

        for (float i = recoveryDuration; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        cooldownTimer = 0;

        yield break;

    }

    void StopParry()
    {
        BetterJump.bj.StopLastChangeFall();

        MovementController.mC.StopLastChangeSpeed();
    }
}
