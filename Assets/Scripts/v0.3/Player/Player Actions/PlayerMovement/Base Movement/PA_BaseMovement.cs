using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public abstract class PA_BaseMovement : MonoBehaviour
{  
    public GameObject player;
    public PS_ArenaPlayerData ps_Data;
    public Rigidbody rb;
    public Collider playerCollider;

    public PA_BaseMovementAction[] MovementActions;

    public void InitiateMovementActions(PlayerInputActions playerInputAc)
    {
        player = gameObject;
        ps_Data = gameObject.GetComponent<PC_Actions>().ps_Data;
        rb = gameObject.GetComponent<Rigidbody>();
        playerCollider = gameObject.GetComponent<PC_Actions>().PlayerCollider;

        SetMovementActions();

        foreach(PA_BaseMovementAction movement in MovementActions)
        { 
            movement.player = player;
            movement.ps_Data = ps_Data;
            movement.rb = rb;
            movement.SetBind(playerInputAc);
        }
    }

    public void DisableMovementActions(PlayerInputActions playerInputAc)
    {
        foreach(PA_BaseMovementAction movement in MovementActions)
        {
            movement.UnsetBind(playerInputAc);
        }
    }

    public abstract void SetMovementActions();
}


public abstract class PA_BaseMovementAction
{
    public Rigidbody rb;
    public GameObject player;
    public PS_ArenaPlayerData ps_Data;
    public abstract void SetBind(PlayerInputActions playerInputAc);
    public abstract void UnsetBind(PlayerInputActions playerInputAc); 
}

public abstract class PA_BaseMovementActionWASD : PA_BaseMovementAction
{
    public float maxSpeed;
    public float maxAccel;
    public float drag;

    public PA_BaseMovementActionWASD(float newSpeed, float newAccel)
    {
        this.maxSpeed = newSpeed;
        this.maxAccel = newAccel;
    }

    public InputAction running;
    public Vector3 wishDir;
    public Transform orientation;

    public virtual void WASDHandler()
    {
        wishDir = orientation.forward * running.ReadValue<Vector2>().y + orientation.right * running.ReadValue<Vector2>().x;
        ps_Data.wishDir = wishDir;
    }

    public override void SetBind(PlayerInputActions playerInputAc)
    {
        orientation = player.transform.Find("PlayerDir"); 
        running = playerInputAc.Arena.Running;
        running.Enable();
    }

    public override void UnsetBind(PlayerInputActions playerInputAc)
    {
        running.Disable();
    }

    public Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(wishDir, ps_Data.GroundNormal).normalized;
    }
}