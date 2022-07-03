using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Cooldown which are regenerating depending on the player speed
public class PA_FrictionCharge : PA_TimeBasedCharges
{
    public PS_ArenaPlayerData ps_Data;

    public event EventHandler OnStackLoad;

    [Header("Friction Charge Specific")]
    public float ChargeLock;
    
    [NonSerialized] public float fcUpMultiplier = 2;

    public override void ChargeLoading()
    {
        float charge = Time.fixedDeltaTime*ps_Data.FrictionBoost;
        
        chargeTracker = Mathf.Min(chargeTracker + fcUpMultiplier*charge,fullCharge);
        int nextStack = StackForCharge(chargeTracker);
        if(nextStack > stacksTracker)
        {
            OnStackLoad?.Invoke(this, EventArgs.Empty);
            stacksTracker = nextStack;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ps_Data = gameObject.transform.parent.gameObject.GetComponent<PC_Actions>().ps_Data;
    }

    void FixedUpdate()
    {
        if(ps_Data.ChargeLockTracker <= 0)
            ChargeLoad();
    }
}