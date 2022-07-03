using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PA_FrictionAbility : PA_Ability
{
    public PA_FrictionCharge FrictionCharge;

    public event EventHandler<FAbility_UseCDEventArg> OnChargeLockSet;

    

    public override bool PA_CDsReady()
    {
        return base.PA_CDsReady() && FrictionCharge.chargeTracker >= FrictionCharge.baseCharge;
    }

    public override void PA_UseCd()
    {
        base.PA_UseCd();
        FrictionCharge.chargeTracker = FrictionCharge.MinusOneStackCharge(FrictionCharge.chargeTracker);
        FrictionCharge.stacksTracker -= 1;
        float nextChargeLock = Mathf.Max(FrictionCharge.ChargeLock, ps_Data.ChargeLockTracker);
        FrictionCharge.ps_Data.ChargeLockTracker = nextChargeLock;
        OnChargeLockSet?.Invoke(this, new FAbility_UseCDEventArg(nextChargeLock));
    }
}

public class FAbility_UseCDEventArg : EventArgs
{
    public float ChargeLock;

    public FAbility_UseCDEventArg(float newChargeLock)
    {
        this.ChargeLock = newChargeLock;
    }
}
