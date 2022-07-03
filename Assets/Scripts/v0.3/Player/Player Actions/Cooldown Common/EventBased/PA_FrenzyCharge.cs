using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PA_FrenzyCharge : PA_EventBasedCharges
{
    public override void ChargeUpdating(float time, PS_ArenaPlayerData playerData, PA_EventChargesInput eventInput)
    {
        chargeTracker = Mathf.Min(chargeTracker + time*eventInput.value,fullCharge);
        //stacksTracker = StackForCharge(chargeTracker);
    }
}
