using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EP_MaterialMultiplier
{
    public float DynamicFriction;
    public float StaticFriction;
    public float Bounciness;

    public static readonly EP_MaterialMultiplier One = new EP_MaterialMultiplier(1, 1, 1);

    public EP_MaterialMultiplier(float newDynamic, float newStatic, float newBounce)
    {
        this.DynamicFriction = newDynamic;
        this.StaticFriction = newStatic;
        this.Bounciness = newBounce;
    }
}
