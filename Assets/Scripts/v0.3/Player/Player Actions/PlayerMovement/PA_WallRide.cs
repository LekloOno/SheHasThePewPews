using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PA_WallRide : MonoBehaviour
{
    public PS_ArenaPlayerData ps_Data;
    public PlayerInputActions playerInputAc;
    public PC_Controller pc_Controller;
    public Rigidbody rb;
    public InputAction running;
    public Vector3 wishDir;
    public Transform orientation;
    public Transform player;

    public LayerMask Ground;

    public float maxSpeed;
    public float maxAccel;

    float otMask;
    Collider currentCollider;

    public float WallRideLength;
    public float flatTimeSpent;
    float wallRideTracker;

    public float wallBoostDecay;
    float realForce;
    float startTime = 0;
    float wallKickStartTime = 0;
    bool sameWall;
    float kickRealForce;
    Collider previousWall;
    float speedStrength;

    public float WallKickStrength;
    public float WallEndStrength;
    Vector3 wallNormal;
    Vector3 velStored;

    public event EventHandler OnFixedUpdate;

    void FixedUpdate()
    {
        OnFixedUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void OnStartWallRide(InputAction.CallbackContext obj)
    { 
        if(ps_Data.WallRideCollisions>0 && !ps_Data.IsSliding && !ps_Data.IsGrounded)
        {
            if(previousWall == currentCollider)
                realForce = Mathf.Pow(Mathf.Min(wallBoostDecay, Time.time-startTime)/wallBoostDecay,2);
            else
            {
                realForce = 1;
                wallKickStartTime = 0;
            }
            previousWall = currentCollider;
            startTime = Time.time;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            ps_Data.IsWallRiding = true;
            rb.useGravity = false;
            speedStrength = 1;

            OnFixedUpdate += OnWallRide;
            wallRideTracker = WallRideLength;
        }
    }

    public void OnStopWallRide(InputAction.CallbackContext obj)
    {
        if(ps_Data.IsWallRiding)
        {
            StopWallRide(true);
        }
    }

    void StopWallRide(bool cancel)
    {
        kickRealForce = Mathf.Pow(Mathf.Min((wallBoostDecay), Time.time-wallKickStartTime)/(wallBoostDecay),2);
        wallKickStartTime = Time.time;
        ps_Data.IsWallRiding = false;
        OnFixedUpdate -= OnWallRide;
        if(cancel)     
        {
            Debug.Log("canceled");
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(((orientation.forward+wallNormal*3f).normalized+5f*Vector3.up)*WallKickStrength*kickRealForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("not canceled");
            rb.velocity = new Vector3(rb.velocity.x*0.8f, 0.2f*rb.velocity.y, rb.velocity.z*0.8f);
            rb.AddForce(wallNormal.normalized*WallEndStrength*kickRealForce, ForceMode.Impulse);
        }
        
        rb.useGravity = true;
    }

    void OnWallRide(object sender, EventArgs e)
    {
        if(wallRideTracker > 0 && !ps_Data.IsGrounded && ps_Data.WallRideCollisions > 0)
        {
            flatTimeSpent = (WallRideLength-wallRideTracker)/WallRideLength;
            wallNormal = player.position - currentCollider.ClosestPoint(player.position);
            ps_Data.WallFlat = Vector3.Cross(wallNormal, Vector3.up);
            wishDir = orientation.forward * running.ReadValue<Vector2>().y + orientation.right * running.ReadValue<Vector2>().x;
            wishDir = (Vector3.Dot(ps_Data.WallFlat, wishDir)*ps_Data.WallFlat).normalized;
            ps_Data.wishDir = wishDir;
            rb.AddForce(Mathf.Max(Mathf.Min(realForce*maxSpeed - Vector3.Dot(wishDir, ps_Data.Velocity), maxAccel*realForce*maxSpeed*Time.fixedDeltaTime),0)*wishDir, ForceMode.VelocityChange);
            //velStored = rb.velocity;
            //rb.AddForce(Physics.gravity * Mathf.Pow(flatTimeSpent, 3) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //rb.AddForce(Physics.gravity * Mathf.Min(Mathf.Pow(1.5f*flatTimeSpent, 2),1) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddForce(Physics.gravity * Mathf.Pow(flatTimeSpent,1.5f) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            wallRideTracker -= Time.fixedDeltaTime;
        }
        else
        {
            StopWallRide(false);    
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentCollider = other;
        otMask = (int)Mathf.Pow(2,other.gameObject.layer);
        if(otMask == Ground.value)
        {
            ps_Data.WallRideCollisions += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        otMask = (int)Mathf.Pow(2,other.gameObject.layer);
        if(otMask == Ground.value)
        {
            ps_Data.WallRideCollisions -= 1;
        }
    }
}
