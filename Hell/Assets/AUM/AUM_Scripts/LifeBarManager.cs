using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarManager : MonoBehaviour
{
    public List<Animator> heartAnimators;

    public UnityEngine.UI.Slider bloodBarSlider;

    public Animator fillSliderAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bloodBarSlider.value = (float)BloodManager.bm.bloodNumb / (float)BloodManager.bm.bloodNumbMax;

        if( (float)BloodManager.bm.bloodNumb < (float)BloodManager.bm.bloodNumbMax / 3f )
        {
            fillSliderAnimator.SetBool("Allumed", false);
        }
        else
        {
            fillSliderAnimator.SetBool("Allumed", true);
        }

        for(int i = 1; i <= heartAnimators.Count; i++)
        {
            if(HealthManager.hm.life >= i)
            {
                heartAnimators[i - 1].SetBool("Allumed", true);
            }
            else
            {
                heartAnimators[i - 1].SetBool("Allumed", false);
            }
        }
    }
}
