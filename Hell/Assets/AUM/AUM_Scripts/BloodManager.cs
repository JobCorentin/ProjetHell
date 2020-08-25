using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager bm;

    public int bloodNumbMax;
    public int bloodNumb;

    public float speedMultiplier1;
    public float speedMultiplier2;

    // Start is called before the first frame update
    void Start()
    {
        bloodNumb = 0;

        bm = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (bloodNumb == bloodNumbMax)
        {
            MovementController.mC.speedMultiplier = speedMultiplier1;
        }
        else if(bloodNumb >= bloodNumbMax / 2f)
        {
            MovementController.mC.speedMultiplier = speedMultiplier2;
        }
        else if (bloodNumb < bloodNumbMax / 2f)
        {
            MovementController.mC.speedMultiplier = 1f;
        }*/

    }
}
