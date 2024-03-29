using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PMA_Slide : PMA_Ability<DATA_Slide>
{
    [Header("Specifics")]
    public bool IsActive;

    public event EventHandler OnSlideStarted;
    public event EventHandler OnSlideStoped;

    [SerializeField] Transform player;
    [SerializeField] Transform playerDir;
    [SerializeField] PSS_Capsule pss_Capsule;
    [SerializeField] PMA_GroundControl groundControl;
    [SerializeField] PMA_AirControl airControl;
    [SerializeField] PMA_GroundControl crouchControl;
    [SerializeField] PMA_AirControl airSlideControl;
    [SerializeField] PMA_SuperJump _superjumpHandler;

    [Header("Specifics/Behavior")]
    //[SerializeField] float crouchMaxSpeed;
    [SerializeField] float yScaleDownSpeed;
    [SerializeField] float yScaleUpSpeed;
    [SerializeField] float slideYScale;

    float startTime = 0;

    float currentScale = 1;
    float scaleSpeed;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Scaling();
    }

    public override void StartAbility(InputAction.CallbackContext obj)
    {
        float realForce = Mathf.Pow(Mathf.Min(data.SlideDecayRecover, Time.time-startTime)/data.SlideDecayRecover,data.SlideDecayStrength);
        startTime = Time.time;
        InitiateSlide();
        rb.AddForce(inputHandler.WishDir * data.SlideXForce * realForce * (spaceState.IsGrounded ? 1 : data.AirForceMultiplier), ForceMode.Impulse);
        OnSlideStarted?.Invoke(this, EventArgs.Empty);
    }

    public void InitiateSlide()
    {
        Activate(true);
        currentScale = slideYScale;
        scaleSpeed = yScaleDownSpeed;
        pss_Capsule.CurrentBaseMat = data.SlideMat;
        rb.drag = 0;
        OnFixedUpdate += UpdateCrouch;
    }

    public override void StopAbility(InputAction.CallbackContext obj)
    {
        if(data.SlideDecayRecover-Time.time+startTime < data.SlideDecayMinRecover)
        {
            startTime = Time.time-(data.SlideDecayRecover-data.SlideDecayMinRecover);
        }
        _superjumpHandler.ResetCharge(false);
        StopSlide();
    }

    public void StopSlide()
    {
        Activate(false);
        crouchControl.enabled = false;
        currentScale = 1;
        scaleSpeed = yScaleUpSpeed;
        OnSlideStoped?.Invoke(this, EventArgs.Empty);
    }

    protected override void Ability_OnInputActionsEnable(object sender, EventArgs e)
    {
        inputHandler.playerInputActions.Arena.Slide.performed += StartAbility;
        inputHandler.playerInputActions.Arena.Slide.canceled += StopAbility;
    }

    protected override void Ability_OnInputActionsDisable(object sender, EventArgs e)
    {
        inputHandler.playerInputActions.Arena.Slide.performed -= StartAbility;
        inputHandler.playerInputActions.Arena.Slide.canceled -= StopAbility;
    }

    void Activate(bool state)
    {
        IsActive = state;
        groundControl.enabled = !state;
        airControl.enabled = !state;
        _superjumpHandler.enabled = state;
        airSlideControl.enabled = state;
    }

    void Scaling()
    {
        player.localScale = new Vector3(player.localScale.x, Mathf.Lerp(player.localScale.y, currentScale, scaleSpeed), player.localScale.z);
    }

    public void UpdateCrouch(object sender, EventArgs e)
    {
        if(IsActive)
        {
            if(spaceState.FlatVelocity <= groundControl.Data.MaxSpeed*0.75f && spaceState.IsGrounded)
            {
                crouchControl.enabled = true;
                OnFixedUpdate -= UpdateCrouch;
            }
        }
        else
        {
            crouchControl.enabled = false;
            OnFixedUpdate -= UpdateCrouch;
        }
    }
}
