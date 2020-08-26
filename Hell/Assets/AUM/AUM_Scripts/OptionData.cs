using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : MonoBehaviour
{
    static public OptionData od;

    public bool shaking;

    private void Start()
    {
        od = this;
        shaking = true;
    }
}
