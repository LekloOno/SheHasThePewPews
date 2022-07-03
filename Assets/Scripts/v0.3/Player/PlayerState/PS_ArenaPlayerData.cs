using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PS_Data", menuName = "Player State/Data")]
public class PS_ArenaPlayerData : ScriptableObject
{
    public GameObject player;
    //Movements
    public bool IsJumping;
    public bool IsSliding;
    public bool IsGrounded;
    public bool CanJump;
    public bool IsSlopeSliding;
    public bool IsNextToWall;
    public bool IsWallRiding;

    public int WallRideCollisions;
    public Vector3 WallFlat;

    public float FlatVelocity;
    public Vector3 Velocity;
    public float FrictionBoost;
    public float GroundHeight;

    public Vector3 wishDir;
    public Vector3 GroundNormal;
    public float GroundAngle;

    public float cameraZrotation;
    //
    public float Health;
    public float Shield;

    public float ChargeLockTracker = 0;
}
