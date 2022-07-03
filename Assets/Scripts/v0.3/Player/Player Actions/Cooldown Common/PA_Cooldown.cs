using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*

    ABSTRACT

Common definitions

*/

[Serializable]
public abstract class PA_Cooldown : MonoBehaviour
{
    [NonSerialized] public float chargeTracker;
    [NonSerialized] public int stacksTracker;

    public float baseCharge;                            // The charge required for the first stack (or simply the charge required for non stackable SpeedCharges)
    [NonSerialized] public float fullCharge;            // For stackable SpeedCharges - The maximum charge stored/The charge required for the last stack.

    public int stacks = 1;                              // must be >= 1 - 1 means it's a non stackable SpeedCharge, n means you can stack n charges.
    public float stacksMultiplier = 1;                  // For stackable SpeedCharges - must be > 0 - 1 means it's the same amount for each stack, < 1 means it'll be less and less for higher stacks, > 1 means it'll be more and more for higher stacks.

    public float FullCharge()
    {
        if(stacks == 1)
        {
            return baseCharge;
        }
        else if(stacksMultiplier == 1){
            return stacks*baseCharge;
        }
        else {
            return (baseCharge*(1-Mathf.Pow(stacksMultiplier,stacks)))/(1-stacksMultiplier);
        }
    }

    public float ChargeOfStack(float stack)
    {
    /*  Returns the charge requires for this object "stack" stack.
    https://www.desmos.com/calculator/vsoytzl5wj
    */
        if(stack == 1)
        {
            return baseCharge;
        }
        else if(stack == stacks)
        {
            return fullCharge;
        }
        else if(stacksMultiplier == 1){
            return stack*baseCharge;
        }
        else {
            return (baseCharge*(1-Mathf.Pow(stacksMultiplier,stack)))/(1-stacksMultiplier);
        }
    }

    public float StackForChargeToFloat(float charge)
    {
        if(charge == fullCharge)
        {
            return stacks;
        }
        else if(stacksMultiplier == 1){
            return charge/baseCharge;
        }
        else {
            return Mathf.Log(((stacksMultiplier-1)*charge)/baseCharge+1)/Mathf.Log(stacksMultiplier);
        }
    }

    public int StackForCharge(float charge)
    {
    /*  Returns the charged stack of this object with a given charge.
    https://www.desmos.com/calculator/vsoytzl5wj
    */
        return Mathf.FloorToInt(StackForChargeToFloat(charge));
    }

    public float MinusOneStackCharge(float charge)
    {
        return ChargeOfStack(StackForChargeToFloat(charge)-1);
    }

    protected virtual void Awake()
    {
        fullCharge = FullCharge();
    }
}


public abstract class PA_TimeBasedCharges : PA_Cooldown
//Common base for every cooldown that will be called to be loaded over time : ChargeLoading acts as a modifier, using the data of playerData.
{
    public void ChargeLoad()
    {
        if (chargeTracker < fullCharge || stacksTracker != stacks)
        {
            ChargeLoading();
        }
    }

    public abstract void ChargeLoading();
}

public abstract class PA_EventBasedCharges : PA_Cooldown
//Common base for every cooldown that will be called to be loaded on specific events : ex- dealing damage
{
    public void ChargeUpdate(float value, PS_ArenaPlayerData playerData, PA_EventChargesInput eventInput)
    {
        if (chargeTracker < fullCharge || stacksTracker != stacks)
        {
            ChargeUpdating(value, playerData, eventInput);
        }
    }

    public abstract void ChargeUpdating(float value, PS_ArenaPlayerData playerData, PA_EventChargesInput eventInput);
}

public struct PA_EventChargesInput
{
    public float value;
}