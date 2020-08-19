using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarZoneManager : MonoBehaviour
{
    public int currentArea;
    public List<GameObject> poses;


    public void ChangingArea(int place)
    {
        if (place != currentArea)
        {
            poses[place].GetComponent<ChangeAstarPose>().SetAreaPosition();
            currentArea = place;
        }
    }
}
