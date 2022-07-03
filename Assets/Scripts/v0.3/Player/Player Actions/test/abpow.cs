using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class abpow : ScriptableObject
{
    public float baseCharge;                    // The charge required for the first stack (or simply the charge required for non stackable SpeedCharges)
    [System.NonSerialized] public float fullCharge;    // For stackable SpeedCharges - The maximum charge stored/The charge required for the last stack.

    public int stacks;              // >= 1 - 1 means it's a non stackable SpeedCharge, n means you can stack n charges.
    public float stacksMultiplier;  // For stackable SpeedCharges -  > 0 - 1 means it's the same amount for each stack, < 1 means it'll be less and less for higher stacks, > 1 means it'll be more and more for higher stacks.

    public float chargeTracker;
    public float stacksTracker;

    public float FullCharge(float baseCharge, float stacksMultiplier, int stacks)
    {
        return (baseCharge*(1-Mathf.Pow(stacksMultiplier,stacks)))/(1-stacksMultiplier);
    }

    public float ChargeOfStack(int stack)
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

    public int StackForCharge(float charge)
    {
    /*  Returns the charged stack of this object with a given charge.
    https://www.desmos.com/calculator/vsoytzl5wj
    */
        if(charge == fullCharge)
        {
            return stacks;
        }
        else if(stacksMultiplier == 1){
            return Mathf.FloorToInt(charge/baseCharge);
        }
        else {
            return Mathf.FloorToInt(Mathf.Log(((stacksMultiplier-1)*charge)/baseCharge+1)/Mathf.Log(stacksMultiplier));
        }
    }
}