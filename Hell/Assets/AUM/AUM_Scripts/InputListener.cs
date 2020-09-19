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

    bool xBoxController;

    // Start is called before the first frame update
    void Awake()
    {
        iL = this;
    }

    // Update is called once per frame
    void Update()
    {
        xBoxController = true;

        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                xBoxController = false;
            }
            if (names[x].Length == 33)
            {
                xBoxController = true;
            }
        }
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        inputVector = new Vector2(horizontalInput, verticalInput);

        if (inputVector.magnitude != 0)
        {
            directionVector = inputVector;
        }

        if(xBoxController == true)
        {
            if (!jumpInput)
                jumpInput = Input.GetButtonDown("Jump");

            if (!attackInput)
                attackInput = Input.GetButtonDown("Attack");

            parryInput = Input.GetButton("Parry");
        }
        else
        {
            if (!jumpInput)
                jumpInput = Input.GetButtonDown("Jump");

            if (!attackInput)
                attackInput = Input.GetButtonDown("Parry");

            parryInput = Input.GetButton("Attack");
        }

        

        /*if (!bloodModeInput)
            bloodModeInput = Input.GetButtonDown("BloodMode");*/
    }

    void FixedUpdate()
    {
        
    }
}
