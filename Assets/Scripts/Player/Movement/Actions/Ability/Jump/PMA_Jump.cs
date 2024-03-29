using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PMA_Jump : PMA_Ability<DATA_Jump>
{
    [Header("Specifics/Behavior")]
    [SerializeField] float resetMaxTime = 0.05f;

    [Header("Debugging")]
    [SerializeField] float tracker_heldJumpCD = 0;
    [SerializeField] float tracker_heldJumpDelay;
    [SerializeField] float tracker_heldJumpThreshold;
    [SerializeField] float tracker_jumpDecayRecover;

    [SerializeField] bool canJump = true;

    #region Setup

    protected override void Awake()
    {
        base.Awake();
        spaceState.OnLeavingGround += Jump_OnLeavingGround;
    }

    protected override void Ability_OnInputActionsEnable(object sender, EventArgs e)
    {
        this.enabled = true;
        inputHandler.playerInputActions.Arena.Jump.performed += StartAbility;
        inputHandler.playerInputActions.Arena.Jump.canceled += StopAbility;
    }

    protected override void Ability_OnInputActionsDisable(object sender, EventArgs e)
    {
        this.enabled = false;
        inputHandler.playerInputActions.Arena.Jump.performed -= StartAbility;
        inputHandler.playerInputActions.Arena.Jump.canceled -= StopAbility;
    }

    #endregion

    public override void StartAbility(InputAction.CallbackContext obj)
    {
        if(canJump)
        {
            tracker_heldJumpThreshold = data.HeldJumpThreshold;

            OnFixedUpdate += OnJump;
        }
    }

    public override void StopAbility(InputAction.CallbackContext obj)
    {
        OnFixedUpdate -= OnJump;
        tracker_heldJumpCD = 0;
    }

    void OnJump(object sender, EventArgs e)
    {
        if(canJump && spaceState.IsGrounded)
        {
            if(tracker_heldJumpThreshold > 0)
            {
                Jump(data.TapJumpForce);
            }
            else
            {
                if(tracker_heldJumpCD > 0)
                    tracker_heldJumpCD -= Time.fixedDeltaTime;
                else
                {
                    if(tracker_heldJumpDelay > 0)
                        tracker_heldJumpDelay -= Time.fixedDeltaTime;
                    else
                        Jump(data.HeldJumpForce);
                }
            }
        }
        else
        {
            tracker_heldJumpCD -= Time.fixedDeltaTime;
            tracker_heldJumpThreshold -= Time.fixedDeltaTime;
        }
    }

    void Jump(float force)
    {
        canJump = false;
        Invoke("ForceReset", resetMaxTime);


        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y*data.PreYMulltiplier, rb.velocity.z);

        float recoverMultiplier;
        if(data.JumpDecayRecover > 0)
            recoverMultiplier = Mathf.Pow(1-(tracker_jumpDecayRecover/data.JumpDecayRecover), data.JumpDecayStrength);
        else
            recoverMultiplier = 1;

        rb.AddForce(Vector3.up*force*recoverMultiplier, ForceMode.Impulse);

        tracker_heldJumpCD = data.HeldJumpCD;
        tracker_heldJumpDelay = data.HeldJumpDelay;

        float currentRecover = tracker_jumpDecayRecover;
        tracker_jumpDecayRecover = data.JumpDecayRecover;
        if(currentRecover <= 0)
            OnFixedUpdate += RecoverDecay;
    }

    void RecoverDecay(object sender, EventArgs e)
    {
        if(tracker_jumpDecayRecover > 0)
            tracker_jumpDecayRecover -= Time.fixedDeltaTime;
        else
        {
            tracker_jumpDecayRecover = 0;
            OnFixedUpdate -= RecoverDecay;
        }
    }

    public void Jump_OnLeavingGround(object sender, EventArgs e)
    {
        canJump = true;
    }

    void ForceReset()
    {
        canJump = true;
    }
}
