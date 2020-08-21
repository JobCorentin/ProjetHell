using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInitiator : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        if(PlayerData.pd != null)
        if(PlayerData.pd.changePosition == true)
        {
            MovementController.mC.transform.position = PlayerData.pd.position;
        }

        Destroy(gameObject);
    }
}
