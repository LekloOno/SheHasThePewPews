using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PA_SuperJump : PA_FrictionAbility
{
    PA_ArenaMovement aMovement;
    public Transform playerCamera;
    /*
    public override void setBind(PlayerInputActions playerInputActions, int Ability_x = 1)
    {
        playerInputActions.Arena.Jump.started += PA_Started; 
        playerInputActions.Arena.Jump.Enable();
    }

    public override void unsetBind(PlayerInputActions playerInputActions, int Ability_x = 1)
    {
        playerInputActions.Arena.Jump.started -= PA_Started; 
        playerInputActions.Arena.Jump.Disable();
    }*/

    public override void PA_OnStart(InputAction.CallbackContext obj)
    {
        aMovement.Slide.OutStopSlide();
        rb.velocity = new Vector3(rb.velocity.x*0.4f,0, rb.velocity.z*0.4f);
        Vector3 sjforce = (ps_Data.wishDir.normalized+Vector3.up*2)*3.8f;
        //rb.AddForce(MovementPhysics.SuperJumpVecForce(playerCamera.TransformDirection(Vector3.forward).normalized, ps_Data.Velocity.magnitude, ps_Data.GroundHeight, aMovement.S_YScale), ForceMode.Impulse);
        rb.AddForce(sjforce, ForceMode.Impulse);
        Debug.Log(sjforce + " | " + sjforce.magnitude);
    }

    public override bool PA_CanStart()
    {
        return base.PA_CanStart() && ps_Data.GroundHeight>1.5f && ps_Data.WallRideCollisions == 0/*player.GetComponent<PC_Actions>().ps_Data.IsSliding*/;
    }

    protected override void Awake()
    {
        base.Awake();
        aMovement = player.GetComponent<PA_ArenaMovement>();
        playerCamera = player.transform.Find("CameraPosition");
    }
}