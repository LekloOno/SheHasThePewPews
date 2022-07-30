using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PMA_SlopeControl : PMA_SurfaceControl<DATA_SlopeControl>
{
    [Header("Specifics")]
    public bool IsActive = false;

    [SerializeField] PMA_GroundControl groundControl;
    [SerializeField] PMA_AirControl airControl;
    [SerializeField] PSS_Capsule pss_Capsule;
    
    Vector3 appliedDir;

    protected override void Awake()
    {
        spaceState.OnEnteringSlope += SurfaceControl_OnLandingSurface;
        spaceState.OnExitingSlope += SurfaceControl_OnLeavingSurface;
    }
    /*
    protected override void OnDisable()
    {
        spaceState.OnEnteringSlope -= SurfaceControl_OnLandingSurface;
        spaceState.OnExitingSlope -= SurfaceControl_OnLeavingSurface;
    }*/

    public override void SurfaceControl_OnLandingSurface(object sender, EventArgs e)
    {
        OnFixedUpdate += OnSloped;
    }

    public override void SurfaceControl_OnLeavingSurface(object sender, EventArgs e)
    {
        OnFixedUpdate -= OnSloped;
        Activate(false);
    }

    void OnSloped(object sender, EventArgs e)
    {
        if(spaceState.FlatVelocity >= data.EnteringSpeed)
        {
            if(!IsActive)
            {
                Activate(true);
                pss_Capsule.CurrentBaseMat = data.Mat;
            }
            float speedCoef = Mathf.Pow((Mathf.Min(data.FullSpeed, spaceState.FlatVelocity)-data.EnteringSpeed)/(data.FullSpeed-data.EnteringSpeed), data.DragTension);
            rb.drag = (1-speedCoef)*data.Drag;
            appliedDir = Vector3.ProjectOnPlane(inputHandler.WishDir, spaceState.GroundNormal).normalized;
            rb.AddForce(MovementPhysics.Acceleration(data.MaxSpeed, data.MaxAccel, rb.velocity, inputHandler.WishDir, appliedDir), ForceMode.VelocityChange);
        }
        else
        {
            if(IsActive)
            {
                Activate(false);
            }
        }
    }

    void Activate(bool state)
    {
        IsActive = state;
        groundControl.enabled = !state;
        airControl.enabled = !state;
    }
}
