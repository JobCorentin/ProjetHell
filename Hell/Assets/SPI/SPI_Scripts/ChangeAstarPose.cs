using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChangeAstarPose : MonoBehaviour
{
    public GridGraph gg;

    public int areaID;
    public AstarZoneManager manager;

    private void Start()
    {

        gg = AstarPath.active.data.gridGraph;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.ChangingArea(areaID);
        }
    }

    public void SetAreaPosition()
    {
        gg.center = gameObject.transform.position;

        AstarPath.active.Scan();
    }
}
