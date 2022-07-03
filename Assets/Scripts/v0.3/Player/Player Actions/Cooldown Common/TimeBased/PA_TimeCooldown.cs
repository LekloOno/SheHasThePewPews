using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Classic cooldowns
public class PA_TimeCooldown : PA_TimeBasedCharges
{
    public override void ChargeLoading()
    {
        chargeTracker = Mathf.Min(chargeTracker + Time.fixedDeltaTime, fullCharge);
        stacksTracker = StackForCharge(chargeTracker);
    }

    void FixedUpdate()
    {
        ChargeLoad();
    }
}
