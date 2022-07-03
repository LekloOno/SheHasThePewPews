using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PC_WallDetection : MonoBehaviour
{
    public LayerMask Ground;
    public PS_ArenaPlayerData ps_Data;

    int otMask;
    int collisions = 0;

    private void OnTriggerEnter(Collider other)
    {
        if((Ground.value & (1 << other.gameObject.layer)) != 0)
        {
            collisions += 1;
            ps_Data.IsNextToWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if((Ground.value & (1 << other.gameObject.layer)) != 0)
        {
            collisions -= 1;
            if(collisions == 0)
                ps_Data.IsNextToWall = false;
        }
    }
}
