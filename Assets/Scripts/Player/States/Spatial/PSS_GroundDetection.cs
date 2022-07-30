using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSS_GroundDetection : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] PSS_Ground groundHandler;
    [SerializeField] Transform playerPosition;
    void OnTriggerEnter(Collider other)
    {
        if((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            groundHandler.IsGrounded = true;
            groundHandler.GroundCollider = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other == groundHandler.GroundCollider)
        {
            groundHandler.IsGrounded = false;
            groundHandler.GroundCollider = null;
        }
    }
}
