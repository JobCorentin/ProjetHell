using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager fxm;

    public List<GameObject> fxList = new List<GameObject>();

    void Start()
    {
        fxm = this;
    }

    public void fxInstancier(int fxIndex, Transform pos,float zRotation)
    {
        GameObject fx = fxList[fxIndex].gameObject;
        GameObject instantiatedFX = Instantiate(fx, pos.position, Quaternion.identity);
        instantiatedFX.transform.rotation = Quaternion.Euler(0,0,zRotation);

    }
}
