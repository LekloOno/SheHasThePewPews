using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class PA_Ability : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public PS_ArenaPlayerData ps_Data;

    public PA_TimeCooldown TimeCooldown;

    public event EventHandler OnUseCd;
    public event EventHandler OnTCDreset;

    #region Actions

    public virtual void PA_UseCd()
    {
        OnUseCd?.Invoke(this, EventArgs.Empty);
        Invoke("TCDreset", TimeCooldown.baseCharge);

        TimeCooldown.chargeTracker = TimeCooldown.MinusOneStackCharge(TimeCooldown.chargeTracker);
    }

    public void TCDreset()
    {
        OnTCDreset?.Invoke(this, EventArgs.Empty);
    }

    public virtual void PA_OnStart(InputAction.CallbackContext obj){}
    public virtual void PA_OnPerform(InputAction.CallbackContext obj){}
    public virtual void PA_OnCancel(InputAction.CallbackContext obj){}
    public virtual void PA_OnActive(float time){}

    #region Actions Detection
    
    public virtual void PA_Active(float time)
    {
        if(isActive)
        {
            PA_OnActive(time);
        }
    }

    public virtual void PA_Started(InputAction.CallbackContext obj)
    {
        if(PA_CanStart())
        {
            PA_OnStart(obj);
            PA_UseCd();

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

    #endregion

    #endregion

    #region Setup

    #region Bindings

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

    public virtual void unsetBind(PlayerInputActions playerInputActions, int Ability_x = 1)
    {
        switch (Ability_x)
        {
            case 1 :
                playerInputActions.Arena.Ability_1.started -= PA_Started;
                playerInputActions.Arena.Ability_1.performed -= PA_Performed;
                playerInputActions.Arena.Ability_1.canceled -= PA_Canceled;
                playerInputActions.Arena.Ability_1.Disable();
                break;
            case 2 :
                playerInputActions.Arena.Ability_2.started -= PA_Started;
                playerInputActions.Arena.Ability_2.performed -= PA_Performed;
                playerInputActions.Arena.Ability_2.canceled -= PA_Canceled;
                playerInputActions.Arena.Ability_2.Disable();
                break;
        }
    }

    #endregion

    protected virtual void Awake()
    {
        player = gameObject.transform.parent.gameObject;
        rb = player.GetComponent<Rigidbody>();
    }

    #endregion

    #region Status

    [NonSerialized] public bool isActive = false;
    [NonSerialized] public bool canPerform = true;
    [NonSerialized] public bool canCancel = true;

    public virtual bool PA_CanStart()
    {
        return PA_CDsReady();
    }

    public virtual bool PA_CDsReady()
    {
        return TimeCooldown.chargeTracker >= TimeCooldown.baseCharge;
    }

    #endregion
}