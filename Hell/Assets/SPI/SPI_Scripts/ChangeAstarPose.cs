using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChangeAstarPose : MonoBehaviour
{
    public AstarData data;
    GridGraph gg;

    public GameObject newGridPose;


    private void Start()
    {

        gg = data.gridGraph;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Palyer")
        {
            //pathfinder.GetComponent<AstarPath>.center = newGridPose.transform.position;
            gg.center = newGridPose.transform.position;

            //astar.Scan();
        }
    }

}
