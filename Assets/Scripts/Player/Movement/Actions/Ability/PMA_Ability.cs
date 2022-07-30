using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PMA_Ability<T> : PM_BaseAction<T> where T : DATA_BaseAction
{
    public event EventHandler OnFixedUpdate;
    protected InputAction action;

    protected virtual void FixedUpdate()
    {
        OnFixedUpdate?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void Awake()
    {
        inputHandler.OnInputActionsEnable += Ability_OnInputActionsEnable;
    }
/*
    protected virtual void OnEnable()
    {
        inputHandler.OnInputActionsEnable += Ability_OnInputActionsEnable;
    }

    protected virtual void OnDisable()
    {
        inputHandler.OnInputActionsDisable -= Ability_OnInputActionsDisable;
    }*/

    public virtual void StartAbility(InputAction.CallbackContext obj){}

    public virtual void StopAbility(InputAction.CallbackContext obj){}

    protected virtual void Ability_OnInputActionsEnable(object sender, EventArgs e)
    {
    }

    protected virtual void Ability_OnInputActionsDisable(object sender, EventArgs e)
    {
    }
}
