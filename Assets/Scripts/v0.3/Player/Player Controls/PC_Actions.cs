using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PC_Actions : MonoBehaviour
{
    public Rigidbody rb;
    public Collider PlayerCollider;
    public Collider WallCollider;
    public GameObject Body;
    public PA_FrictionAbility[] pa_Abilities;
    public PA_BaseMovement pa_BaseMovement;
    public LayerMask Ground;

    public PS_ArenaPlayerData ps_Data;
    public float distToGround;
    public float distTGAngleMultiplier;
    float finalDistTG;

    RaycastHit groundHit;

    public event EventHandler<ChargeLockSetEventArg> OnNewChargeLock;
    public event EventHandler OnLanding;

    void FixedUpdate()
    {
        foreach(PA_FrictionAbility pa in pa_Abilities)
        {
            pa?.PA_Active(Time.fixedDeltaTime);
            /*
            foreach (PA_TimeBasedCharges c in pa.TimeCharges)
            {
                c.ChargeLoad();
                Debug.Log("Tracker : " + c.chargeTracker);
            }*/
        }
        PlayerStateUpdate();
    }

    void PlayerStateUpdate()
    {
        ps_Data.Velocity = rb.velocity;
        ps_Data.FlatVelocity = (new Vector3(rb.velocity.x,0,rb.velocity.z)).magnitude;
        if(ps_Data.IsGrounded && ps_Data.IsSliding)
            ps_Data.FrictionBoost = SF_GameplayValues.FrictionChargeAmount(ps_Data.FlatVelocity, 4, 0.5f)*0.5f;
        else
            ps_Data.FrictionBoost = SF_GameplayValues.FrictionChargeAmount(ps_Data.FlatVelocity);

        if(ps_Data.ChargeLockTracker > 0)
            ps_Data.ChargeLockTracker -= Time.fixedDeltaTime;

        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, Ground))
        {
            ps_Data.GroundHeight = groundHit.distance;
            finalDistTG = distToGround+Mathf.Min(ps_Data.GroundAngle*distTGAngleMultiplier,45*distTGAngleMultiplier);
            if(ps_Data.IsGrounded != groundHit.distance <= finalDistTG)
            {
                ps_Data.IsGrounded = groundHit.distance <= finalDistTG;
                OnLanding?.Invoke(this, EventArgs.Empty);
            }
            
            if(groundHit.distance <= finalDistTG+0.3f)
            {
                ps_Data.GroundNormal = groundHit.normal;
                ps_Data.GroundAngle = Vector3.Angle(Vector3.up, groundHit.normal);
            }
            else
            {
                ps_Data.GroundNormal = Vector3.up;
                ps_Data.GroundAngle = 0;
            }
        }
    }
    public void ResetParkour(InputAction.CallbackContext obj)
    {
        transform.position = new Vector3(35,19,-28);
    }
    /*
    public void FrictionAbility_OnUseCd(object sender, EventArgs e)
    {
        OnNewChargeLock?.Invoke(this, new ChargeLockSetEventArg(Mathf.Max(ps_Data.ChargeLockTracker, sender.FrictionCharge.ChargeLock)));
    }*/
}

public class ChargeLockSetEventArg : EventArgs
{
    public float ChargeLock;

    public ChargeLockSetEventArg(float newChargeLock)
    {
        this.ChargeLock = newChargeLock;
    }
}