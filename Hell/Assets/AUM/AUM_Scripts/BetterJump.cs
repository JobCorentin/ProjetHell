using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public static BetterJump bj;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private void Start()
    {
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

}
