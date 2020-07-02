using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public static BetterJump bj;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    float originalFallMultiplier;

    public Coroutine lastChangeFall;


    private void Start()
    {
        originalFallMultiplier = fallMultiplier;

        bj = this;
    }

    void Update()
    {
        if(MovementController.mC.projected == false)
        {

            if (MovementController.mC.rb.velocity.y < 0)
            {
                MovementController.mC.rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (MovementController.mC.rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                MovementController.mC.rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        
    }

    public IEnumerator ChangeFallMultiplier(float duration, float newValue)
    {
        fallMultiplier = newValue;

        for (float i = duration; i >= 0; i -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        fallMultiplier = originalFallMultiplier;
    }

    public void StopLastChangeFall()
    {
        if(lastChangeFall != null)
        {
            StopCoroutine(lastChangeFall);

            fallMultiplier = originalFallMultiplier;
        }
    }

}
