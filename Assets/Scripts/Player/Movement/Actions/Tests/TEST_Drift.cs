using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Drift : MonoBehaviour
{
    public float SteerAngle;
    public float Acceleration;
    public float MaxAccel;
    public float MaxSpeed;
    public float Traction;

    [SerializeField] Transform SteerPivot;
    [SerializeField] Rigidbody Rb;
    [SerializeField] PIS_Combat Inputs;

    Vector3 AccelerationForce;

    void FixedUpdate()
    {
        AccelerationForce += SteerPivot.forward * Acceleration * Time.fixedDeltaTime;
        MaxSpeed *= 1+(Time.fixedDeltaTime*Inputs.RunningAxis.y);
        //Debug.Log(Mathf.Max(MaxSpeed-Vector3.Dot(AccelerationForce, Rb.velocity),0));

        Debug.Log((1-1/(.1f*AccelerationForce.magnitude+1)));
        SteerPivot.Rotate(Vector3.up * Inputs.RunningAxis.x * (1-1/(.1f*AccelerationForce.magnitude+1))* 10 * SteerAngle * Time.fixedDeltaTime);
        AccelerationForce = Vector3.ClampMagnitude(AccelerationForce, MaxSpeed);

        AccelerationForce = Vector3.Lerp(AccelerationForce.normalized, SteerPivot.forward, Traction * Time.fixedDeltaTime) * AccelerationForce.magnitude;

        //Debug.DrawRay(transform.position, Rb.velocity, Color.blue);
        Debug.DrawRay(transform.position, AccelerationForce, Color.red);
        Debug.DrawRay(transform.position, SteerPivot.forward * 10, Color.green);
        Debug.Log(Rb.velocity.magnitude);

        //Rb.AddForce(AccelerationForce * (MaxSpeed-(Vector3.Dot(AccelerationForce.normalized, Rb.velocity)/MaxSpeed)));
        Rb.velocity = AccelerationForce;
    }
}
