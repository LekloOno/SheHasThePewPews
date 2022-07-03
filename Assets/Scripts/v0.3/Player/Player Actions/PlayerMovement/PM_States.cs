using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PM_States", menuName = "Player Movements/States")]
public class PM_States : ScriptableObject
{
    public float maxSpeed;
    public float maxAccel;
    public float drag;

    public bool isActive;
}
