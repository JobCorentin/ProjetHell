using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager bm;

    public int bloodNumbMax;
    [HideInInspector] public int bloodNumb;

    public float speedMultiplier1;
    public float speedMultiplier2;

    public TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        bloodNumb = 0;

        bm = this;
        text = GameObject.Find("BloodText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Blood : " + bloodNumb + " / " + bloodNumbMax;

        
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
