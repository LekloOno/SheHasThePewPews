using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PC_ArenaCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraPosition;
    [SerializeField] GameObject playerDir;
    public PS_ArenaPlayerData ps_Data;
    public Rigidbody rb;
    public PA_WallRide pa_WallRide;

    public float globalSensAdjustment;
    public float xSens = 1;
    public float ySens = 1;

    InputAction lookAction;
    Vector2 lookDir;
    float xRotation;
    float yRotation;


    public float slideTiltingAngle;
    public float slideTiltingSpeed;
    public float slideTiltingResetSpeed;

    public float wallTiltingAngle;
    public float wallTiltingSpeed;
    public float wallTiltingResetSpeed;

    public event EventHandler OnFixedUpdate;

    float camBackT;

    // Update is called once per frame
    void Update()
    {
        OnFixedUpdate?.Invoke(this, EventArgs.Empty);
        OnLook();
        transform.position = cameraPosition.transform.position;
        transform.rotation = cameraPosition.transform.rotation;

        if(ps_Data.IsWallRiding)
            wallRideTilting(ps_Data.WallFlat.normalized, playerDir.transform.TransformDirection(-Vector3.forward).normalized, pa_WallRide.flatTimeSpent);
        else if(ps_Data.IsSliding)
            slideTilting(rb.velocity.normalized, playerDir.transform.TransformDirection(Vector3.right).normalized);
        else if((ps_Data.IsSlopeSliding && ps_Data.FlatVelocity > 7f))
            slideTilting(rb.velocity.normalized, playerDir.transform.TransformDirection(Vector3.right).normalized, Mathf.Min(Mathf.Max(1+(ps_Data.FlatVelocity-6.2f)/6.2f, 1), 3));
        else if(ps_Data.cameraZrotation != 0)
            resetSlideTilting();
    }

    public void InitiateLook(PlayerInputActions playerInputAc)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookAction = playerInputAc.Arena.Look;
        //playerInputAc.Arena.Slide.performed += Camera_OnStartSlide;
        lookAction.Enable();
    }

    public void DisableLook(PlayerInputActions playerInputAc)
    {
        lookAction.Disable();
    }

    public void OnLook()
    {
        float lookX = lookAction.ReadValue<Vector2>().x*globalSensAdjustment*xSens;
        float lookY = lookAction.ReadValue<Vector2>().y*globalSensAdjustment*ySens;

        xRotation = Mathf.Clamp(xRotation-lookY, -90f, 90f);

        yRotation += lookX;

        cameraPosition.transform.rotation = Quaternion.Euler(xRotation, yRotation, ps_Data.cameraZrotation);
        playerDir.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void slideTilting(Vector3 velocity, Vector3 fwdDir, float ratio=1)
    {
        float velDirScalar = Vector3.Dot(velocity, fwdDir);
        ps_Data.cameraZrotation += (slideTiltingAngle*ratio*velDirScalar-ps_Data.cameraZrotation)*Time.deltaTime*slideTiltingSpeed;
    }

    public void wallRideTilting(Vector3 normal, Vector3 fwdDir, float ratio)
    {
        Debug.Log(ratio);
        float velDirScalar = Vector3.Dot(normal, fwdDir);
        ps_Data.cameraZrotation += (wallTiltingAngle*velDirScalar*(1-Mathf.Clamp(Mathf.Pow(2.8f*(ratio-0.6f),3),0,1))-ps_Data.cameraZrotation)*Time.deltaTime*wallTiltingSpeed;
    }

    public void resetSlideTilting()
    {
        ps_Data.cameraZrotation += (-ps_Data.cameraZrotation)*Time.deltaTime*slideTiltingResetSpeed;
    }

    /*
    public void Camera_OnStartSlide(InputAction.CallbackContext obj)
    {
        camBackT = 0;
        if(ps_Data.FlatVelocity > 3)
            OnFixedUpdate += CameraBacking;
    }
    
    public void CameraBacking(object sender, EventArgs e)
    {
        camBackT += Time.deltaTime;
        if(camBackT <= 0.4f)
        {
            Debug.Log((Mathf.Abs(5*(camBackT-0.2f))-1)*playerDir.transform.forward);
            cameraPosition.transform.localPosition = (Mathf.Pow(4*camBackT-1,2)-1)*playerDir.transform.forward*0.5f + new Vector3(0,0.7f,0);
        }
        else
            OnFixedUpdate -= CameraBacking; 
    }*/
}
