using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public static InputListener iL;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public Vector2 inputVector;

    [HideInInspector] public Vector2 directionVector;

    [HideInInspector] public bool jumpInput;

    [HideInInspector] public bool attackInput;

    [HideInInspector] public bool parryInput;

    [HideInInspector] public bool bloodModeInput;

    // Start is called before the first frame update
    void Start()
    {
        iL = this;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        inputVector = new Vector2(horizontalInput, verticalInput);

        if (inputVector.magnitude != 0)
        {
            directionVector = inputVector;
        }

        if (!jumpInput)
            jumpInput = Input.GetButtonDown("Jump");

        if (!attackInput)
            attackInput = Input.GetButtonDown("Attack");

        /*if (!parryInput)
            parryInput = Input.GetButton("Parry");*/

        /*if (!bloodModeInput)
            bloodModeInput = Input.GetButtonDown("BloodMode");*/
    }

    void FixedUpdate()
    {
        
    }
}
