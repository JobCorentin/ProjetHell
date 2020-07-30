using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Initator : MonoBehaviour
{
    public Transform fxTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FXInitiator(int fxIndex)
    {
        FXManager.fxm.fxInstancier(fxIndex, fxTransform, 0);
    }
}
