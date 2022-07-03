using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class abpowservice
{
    private static float FullCharge(float baseCharge, float stacksMultiplier, int stacks)
    {
        return (baseCharge*(1-Mathf.Pow(stacksMultiplier,stacks)))/(1-stacksMultiplier);
    }

    public static float ChargeOfStack(int stack, abpow inCharge)
    {
    /*  Returns the charge requires for this object "stack" stack.
    https://www.desmos.com/calculator/vsoytzl5wj
    */
        if(stack == 1)
        {
            return inCharge.baseCharge;
        }
        else if(stack == inCharge.stacks)
        {
            return inCharge.fullCharge;
        }
        else if(inCharge.stacksMultiplier == 1){
            return stack*inCharge.baseCharge;
        }
        else {
            return (inCharge.baseCharge*(1-Mathf.Pow(inCharge.stacksMultiplier,stack)))/(1-inCharge.stacksMultiplier);
        }
    }

    public static int StackForCharge(float charge, abpow inCharge)
    {
    /*  Returns the charged stack of this object with a given charge.
    https://www.desmos.com/calculator/vsoytzl5wj
    */
        if(charge == inCharge.fullCharge)
        {
            return inCharge.stacks;
        }
        else if(inCharge.stacksMultiplier == 1){
            return Mathf.FloorToInt(charge/inCharge.baseCharge);
        }
        else {
            return Mathf.FloorToInt(Mathf.Log(((inCharge.stacksMultiplier-1)*charge)/inCharge.baseCharge+1)/Mathf.Log(inCharge.stacksMultiplier));
        }
    }


    static float fcTopTension = 3.0f;
    static float fcTopOffset = 15.0f;
    static float fcMaxTension = 0.4f;
    static float fcMaxFactor = 0.12f;
    static float fcFactor = 10f;
    static float fcMinimum = fcFactor/40f;

    static float fcUpMultiplier = 1;

    public static float FrictionChargeAmount(float velocity)
    {
    /*
    https://www.desmos.com/calculator/0mt6ixhqbk
    */
        return fcMinimum+fcFactor*(fcMaxFactor*Mathf.Pow(Mathf.Max(velocity-fcTopOffset,0),fcMaxTension)+Mathf.Min(Mathf.Pow((velocity/fcTopOffset),fcTopTension),1));
    }

    public static void FrictionChargeUp(float time, float velocity, abpow inCharge)
    {
        float charge = time*FrictionChargeAmount(velocity);
        
        if (inCharge.chargeTracker < inCharge.fullCharge || inCharge.stacksTracker != inCharge.stacks)
        {
            inCharge.chargeTracker = Mathf.Min(inCharge.chargeTracker + time*fcUpMultiplier*charge,inCharge.fullCharge);
            inCharge.stacksTracker = StackForCharge(inCharge.chargeTracker, inCharge);
        }
        
    }
    
    public static void FrenzyChargeUp(float chargeAmount, float chargeMultiplier, abpow inCharge)
    {
        if (inCharge.chargeTracker < inCharge.fullCharge || inCharge.stacksTracker != inCharge.stacks)
        {
            inCharge.chargeTracker = Mathf.Min(inCharge.chargeTracker + chargeAmount*chargeMultiplier, inCharge.fullCharge);
            inCharge.stacksTracker = StackForCharge(inCharge.chargeTracker, inCharge);
        }
    }
}
