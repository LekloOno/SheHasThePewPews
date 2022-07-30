using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Materials : MonoBehaviour
{
    [SerializeField] PhysicMaterial groundMaterial;
    public EP_MaterialMultiplier WallMultiplier;

    public void MaterialUpdate(EP_MaterialMultiplier multiplier)
    {
        GetComponent<Collider>().material.dynamicFriction = groundMaterial.dynamicFriction * multiplier.DynamicFriction;
        GetComponent<Collider>().material.staticFriction = groundMaterial.staticFriction * multiplier.StaticFriction;
        GetComponent<Collider>().material.bounciness = groundMaterial.bounciness * multiplier.Bounciness; 
    }

    //public void MaterialUpdate() => MaterialUpdate(EP_MaterialMultiplier.One);
    public void MaterialUpdate()
    {
        GetComponent<Collider>().material = groundMaterial;
    }
}
