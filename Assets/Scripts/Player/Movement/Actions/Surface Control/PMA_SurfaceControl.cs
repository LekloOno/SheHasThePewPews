using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PMA_SurfaceControl<T> : PM_BaseAction<T> where T : DATA_SurfaceControl
{

    public event EventHandler OnFixedUpdate;
    
    protected virtual void Awake()
    {
        spaceState.OnLanding += SurfaceControl_OnLandingSurface;
        spaceState.OnLeavingGround += SurfaceControl_OnLeavingSurface;
    }/*
    protected virtual void OnDisable()
    {
        spaceState.OnLanding -= SurfaceControl_OnLandingSurface;
        spaceState.OnLeavingGround -= SurfaceControl_OnLeavingSurface;
    }*/

    public virtual void SurfaceControl_OnLandingSurface(object sender, EventArgs e){}

    public virtual void SurfaceControl_OnLeavingSurface(object sender, EventArgs e){}

    protected void FixedUpdate()
    {
        OnFixedUpdate?.Invoke(this, EventArgs.Empty);
    }
}