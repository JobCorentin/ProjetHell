using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXAnimator : MonoBehaviour
{

    public void instantiateFX(int index,Transform pos)
    {
        FXManager.fxm.fxInstancier(index, pos,0);
    }

}
