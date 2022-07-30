using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PSS_WallDetection : MonoBehaviour
{
    [SerializeField] PSS_Wall wallHandler;
    [SerializeField] PSS_Ground groundHandler;
    [SerializeField] LayerMask ground;
    [SerializeField] PSS_Capsule pss_Capsule;
    [SerializeField] Collider playerCollider;

    List<Collider> CollisionList = new List<Collider>();
    Collider previousGround;
    /*
    private void OnTriggerEnter(Collider other)
    {
        matHandler = other.gameObject.GetComponent<EP_Materials>();

        if((ground.value & (1 << other.gameObject.layer)) != 0 && matHandler != null)
        {
            wallHandler.WallCollisions += 1;
            wallHandler.IsNextToWall = true;
            matHandler.MaterialUpdate(matHandler.WallMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        matHandler = other.gameObject.GetComponent<EP_Materials>();
        
        if((ground.value & (1 << other.gameObject.layer)) != 0 && matHandler != null)
        {
            wallHandler.WallCollisions -= 1;
            matHandler.MaterialUpdate();
            if(wallHandler.WallCollisions == 0)
                wallHandler.IsNextToWall = false;
        }
    }*/
    void OnEnable()
    {
        groundHandler.OnUpdatingGround += WallDetection_OnUpdatingGround;
        groundHandler.OnUpdatingGroundCollider += WallDetection_OnUpdatingGroundCollider;
    }

    void OnDisable()
    {
        groundHandler.OnUpdatingGround -= WallDetection_OnUpdatingGround;
        groundHandler.OnUpdatingGroundCollider -= WallDetection_OnUpdatingGroundCollider;
    }

    private void FixedUpdate()
    {
        if(CollisionList.Contains(groundHandler.GroundCollider))
        {
            CollisionList.Remove(groundHandler.GroundCollider);
            previousGround = groundHandler.GroundCollider;
            wallHandler.WallCollisions -= 1;
            wallHandler.IsNextToWall = wallHandler.WallCollisions != 0;
            SetPlayerMat();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            if(other.collider != groundHandler.GroundCollider)
            {
                CollisionList.Add(other.collider);
                wallHandler.WallCollisions += 1;
                wallHandler.IsNextToWall = true;
                //SetPlayerMat();
            }
            else
            {
                previousGround = other.collider;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(previousGround == other.collider)
        {
            previousGround = null;
        }        
        if(CollisionList.Contains(other.collider))
        {
            CollisionList.Remove(other.collider);
            wallHandler.WallCollisions -= 1;
            wallHandler.IsNextToWall = wallHandler.WallCollisions != 0;
            //SetPlayerMat();
        }
    }

    private void SetPlayerMat()
    {
        if(groundHandler.IsGrounded)
        {
            playerCollider.material.dynamicFriction = pss_Capsule.CurrentBaseMat.dynamicFriction/(Mathf.Pow(wallHandler.WallCollisions+1,2));
            playerCollider.material.staticFriction = pss_Capsule.CurrentBaseMat.staticFriction/(Mathf.Pow(wallHandler.WallCollisions+1,2));
        }
        else
        {
            playerCollider.material.dynamicFriction = 0;
            playerCollider.material.staticFriction = 0;
        }
    }

    public void WallDetection_OnUpdatingGround(object sender, EventArgs e)
    {
        SetPlayerMat();
    }

    public void WallDetection_OnUpdatingGroundCollider(object sender, EventArgs e)
    {
        if(previousGround != null)
        {
            CollisionList.Add(previousGround);
            wallHandler.WallCollisions += 1;
            wallHandler.IsNextToWall = true;
        }
    }
}
