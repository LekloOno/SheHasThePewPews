using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PMA_SuperJump : PMA_Ability<DATA_SuperJump>
{
    [Header("Specifics/Behavior")]
    bool _canJump = true;

    float _tracker_superJumpCharge = 0;
    float _start_superJump = Mathf.NegativeInfinity;
    public float Tracker_superJumpCharge
    {
        get => _tracker_superJumpCharge;
    }
    public float FullChargeTime
    {
        get => data.FullChargeTime;
    }

    #region Setup

    protected override void Awake()
    {
        base.Awake();
        spaceState.OnLanding += SuperJump_OnLanding;
        spaceState.OnLeavingGround += SuperJump_OnLeavingGround;
    }

    protected override void Ability_OnInputActionsEnable(object sender, EventArgs e)
    {
        inputHandler.playerInputActions.Arena.Jump.performed += StartAbility;
    }

    protected override void Ability_OnInputActionsDisable(object sender, EventArgs e)
    {
        this.enabled = false;
        inputHandler.playerInputActions.Arena.Jump.performed -= StartAbility;
    }

    #endregion

    public override void StartAbility(InputAction.CallbackContext obj)
    {
        if(_canJump && spaceState.IsGrounded && this.enabled)
        {
            _canJump = false;
            float recoverMultiplier;
            if(data.DecayRecover > 0)
                recoverMultiplier = Mathf.Pow(Mathf.Min(Time.time-_start_superJump,data.DecayRecover)/data.DecayRecover, data.DecayStrength);
            else
                recoverMultiplier = 1;

            float chargedForce = Mathf.Pow(Mathf.Min(_tracker_superJumpCharge/data.FullChargeTime, 1),data.ChargeCurveStrength)*data.FullChargeForce;
            chargedForce *= recoverMultiplier;
            rb.AddForce(Vector3.up*chargedForce, ForceMode.Impulse);
            _start_superJump = Time.time;

            ResetCharge();
        }
    }

    void SuperJump_OnCharging(object sender, EventArgs e)
    {
        _tracker_superJumpCharge += Time.fixedDeltaTime;
    }

    public void SuperJump_OnLanding(object sender, EventArgs e)
    {
        OnFixedUpdate += SuperJump_OnCharging;
    }

    public void SuperJump_OnLeavingGround(object sender, EventArgs e)
    {
        _canJump = true;
        OnFixedUpdate -= SuperJump_OnCharging;
        Invoke("ResetCharge", data.ChargeAutoResetTime);
    }

    public void ResetCharge()
    {
        ResetCharge(true);
    }

    public void ResetCharge(bool auto)
    {
        if(!auto)
            _tracker_superJumpCharge = 0;
        else if(!spaceState.IsGrounded)
            _tracker_superJumpCharge = 0;
    }
}
