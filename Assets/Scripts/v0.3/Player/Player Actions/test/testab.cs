using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class testab : ScriptableObject
{
    [NonSerialized] public bool isActive = false;
    [NonSerialized] public bool canStart = true;
    [NonSerialized] public bool canPerform = true;
    [NonSerialized] public bool canCancel = true;

    public virtual void PA_OnStart(InputAction.CallbackContext obj){}
    public virtual void PA_OnPerform(InputAction.CallbackContext obj){}
    public virtual void PA_OnCancel(InputAction.CallbackContext obj){}
    public virtual void PA_OnActive(float time){}


    public virtual void PA_Active(float time)
    {
        if(isActive)
        {
            PA_OnActive(time);
        }
    }

    public virtual void PA_Started(InputAction.CallbackContext obj)
    {
        if(canStart)
        {
            PA_OnStart(obj);
            isActive = true;
        }
    }

    public virtual void PA_Performed(InputAction.CallbackContext obj)
    {
        if(canPerform)
        {
            PA_OnPerform(obj);
            isActive = true;
        }
    }
    public virtual void PA_Canceled(InputAction.CallbackContext obj)
    {
        if(canCancel)
        {
            PA_OnCancel(obj);
            isActive = false;
        }
    }

    public virtual void setBind(PlayerInputActions playerInputActions, int Ability_x = 1)
    {
        switch (Ability_x)
        {
            case 1 :
                playerInputActions.Arena.Ability_1.started += PA_Started;
                playerInputActions.Arena.Ability_1.performed += PA_Performed;
                playerInputActions.Arena.Ability_1.canceled += PA_Canceled;
                playerInputActions.Arena.Ability_1.Enable();
                break;
            case 2 :
                playerInputActions.Arena.Ability_2.started += PA_Started;
                playerInputActions.Arena.Ability_2.performed += PA_Performed;
                playerInputActions.Arena.Ability_2.canceled += PA_Canceled;
                playerInputActions.Arena.Ability_2.Enable();
                break;
        }
    }
}


public class testpow : testab
{
    public abpow[] charges;
}

public class testcd : testab
{
    public PA_Cooldown cooldown;
}