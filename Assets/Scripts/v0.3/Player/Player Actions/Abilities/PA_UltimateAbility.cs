using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PA_UltimateAbility : PA_Ability
{
    public PA_FrenzyCharge FrenzyCharge;

    public override bool PA_CDsReady()
    {
        return base.PA_CDsReady() && FrenzyCharge.chargeTracker >= FrenzyCharge.baseCharge;
    }

    public override void PA_UseCd()
    {
        base.PA_UseCd();
        FrenzyCharge.chargeTracker = FrenzyCharge.MinusOneStackCharge(FrenzyCharge.chargeTracker);
    }
}