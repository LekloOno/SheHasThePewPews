using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "abfric", menuName = "test/abfric")]
public class abfric : abpow
{
    float fcTopTension = 3.0f;
    float fcTopOffset = 15.0f;
    float fcMaxTension = 0.4f;
    float fcMaxFactor = 0.12f;
    float fcFactor = 10f;
    float fcMinimum = 40f;

    public float fcUpMultiplier = 1;

    public float FrictionChargeAmount(float velocity)
    {
    /*
    https://www.desmos.com/calculator/0mt6ixhqbk
    */
        return fcFactor/fcMinimum+fcFactor*(fcMaxFactor*Mathf.Pow(Mathf.Max(velocity-fcTopOffset,0),fcMaxTension)+Mathf.Min(Mathf.Pow((velocity/fcTopOffset),fcTopTension),1));
    }

    public void FrictionChargeUp(float time, float velocity)
    {
        float charge = time*FrictionChargeAmount(velocity);
        
        if (chargeTracker < fullCharge || stacksTracker != stacks)
        {
            chargeTracker = Mathf.Min(chargeTracker + time*fcUpMultiplier*charge,fullCharge);
            stacksTracker = StackForCharge(chargeTracker);
        }
        
    }
}