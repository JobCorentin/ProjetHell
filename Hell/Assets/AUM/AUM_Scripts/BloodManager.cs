using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager bm;

    public int bloodNumbMax;
    [HideInInspector] public int bloodNumb;

    public TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        bloodNumb = bloodNumbMax / 2;

        bm = this;
        text = GameObject.Find("BloodText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Blood : " + bloodNumb + " / " + bloodNumbMax;
    }
}
