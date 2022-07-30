using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TEST_Drift2 : MonoBehaviour
{
    [SerializeField] Transform SteerPivot;
    [SerializeField] Rigidbody Rb;
    [SerializeField] PIS_Combat Inputs;
    [SerializeField] Transform Cam;
    public float SteerAngle;

    public bool IsDrifting = false;

    public float Speed;

    public float PivotDrag;

    public float MaxSpeed;
    public float MaxAccel;

    void OnEnable()
    {
        Inputs.OnInputActionsEnable += OnInputEnable;
    }

    void OnInputEnable(object sender, EventArgs e)
    {
        Inputs.playerInputActions.Arena.Slide.performed += Thingy;
    }
    
    void FixedUpdate()
    {
        if(IsDrifting)
        {
            Rb.drag = 2;
            //SteerPivot.Rotate(Vector3.up * Inputs.RunningAxis.x * SteerAngle * Time.fixedDeltaTime);
            SteerPivot.LookAt(transform.position + Cam.forward);
            Vector3 perp = Vector3.Cross(Vector3.up, SteerPivot.forward).normalized;
            Debug.DrawRay(transform.position, -Vector3.Dot(Rb.velocity, perp)*perp);
            Rb.AddForce(-Vector3.Dot(Rb.velocity, perp)*perp*Time.fixedDeltaTime*PivotDrag + MovementPhysics.Acceleration(MaxSpeed, MaxAccel, Rb.velocity, Inputs.WishDir, Inputs.WishDir), ForceMode.VelocityChange);
        }
        else
        {
            Rb.drag = 10;
            Rb.AddForce(Speed*Inputs.WishDir*Time.fixedDeltaTime);
        }

        Debug.DrawRay(transform.position, Rb.velocity, Color.red);
        Debug.DrawRay(transform.position, SteerPivot.forward * 10, Color.green);
        Debug.DrawRay(transform.position, Cam.forward * 10, Color.blue);
        Debug.Log(Rb.velocity.magnitude);
    }

    public void Thingy(InputAction.CallbackContext e)
    {
        Vector3 velDir = new Vector3(Rb.velocity.x,0,Rb.velocity.z);
        SteerPivot.LookAt(transform.position+velDir);
        IsDrifting = !IsDrifting;
    }
}
