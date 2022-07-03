using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Reflection;

public class PA_ArenaMovement : PA_BaseMovement
{
    [Header("Ground Movement Settings")]
    public float G_MaxSpeed;
    public float G_MaxAccel;
    public float G_Drag;
    public float GS_MaxSpeed;
    public float GS_MaxAccel;
    [Header("Air Control Settings")]
    public float A_MaxSpeed;
    public float A_MaxAccel;
    [Header("Jump")]
    public float J_Force;
    public float J_PointForce;
    public float J_HoldCD;
    public float J_AntiSpamCD;
    public float J_Delay;
    public float J_DelayThreshold;

    [Header("Slide")]
    public float S_YScale;
    public float S_XForce;
    public float S_YForce;
    public float S_Drag;
    public float S_AirSpeed;
    public float S_AirAccel;
    public float S_SlideBoostDecay;


    public PhysicMaterial GroundMat;
    public PhysicMaterial SlideMat;
    public PhysicMaterial WallMat;
    public PhysicMaterial WallRideMat;

    public PA_ArenaMovementRunning Running;
    public PA_ArenaMovementAirControl AirControl;
    public PA_ArenaMovementJump Jump;
    public PA_ArenaMovementSlide Slide;

    public event EventHandler<A_MvmtEventArg> OnFixedUpdate;

    public override void SetMovementActions()
    {
        Running = new PA_ArenaMovementRunning(this, G_MaxSpeed, G_MaxAccel, G_Drag, GS_MaxSpeed, GS_MaxAccel);
        AirControl = new PA_ArenaMovementAirControl(A_MaxSpeed, A_MaxAccel);
        Jump = new PA_ArenaMovementJump(this, J_Force, J_PointForce, J_HoldCD, J_AntiSpamCD, J_Delay, J_DelayThreshold);
        Slide = new PA_ArenaMovementSlide(this, S_AirSpeed, S_AirAccel, S_YScale, S_XForce, S_YForce, S_Drag, S_SlideBoostDecay);

        MovementActions = new PA_BaseMovementAction[]
        {
            Jump,
            Slide,
            Running,
            AirControl
        };
    }

    void FixedUpdate()
    {
        OnFixedUpdate?.Invoke(this, new A_MvmtEventArg());
        /*
        Debug.Log(Jump.jumpHoldTracker);
        if(!ps_Data.CanJump && ps_Data.IsJumping)
        {
            Jump.jumpHoldTracker += Time.fixedDeltaTime;
            ps_Data.CanJump = Jump.jumpHoldCD <= Jump.jumpHoldTracker;
        }
        else if(ps_Data.IsJumping)
        {   

            //Slide.OutStopSlide();
            if(ps_Data.IsGrounded)
                Jump.Jump();
        }*/
        if(ps_Data.IsGrounded)
        {
            if(ps_Data.IsSliding)
            {
                if(ps_Data.IsNextToWall)
                    playerCollider.material = WallMat;
                else
                    playerCollider.material = SlideMat;
            }
            else
            {
                Running.WASDHandler();
            }
        }
        else
        {
            playerCollider.material = WallMat;
            ps_Data.IsSlopeSliding = false;
            if(ps_Data.IsWallRiding)
            {

            }
            else if(ps_Data.IsSliding)
            {
                Slide.WASDHandler();
            }     
            else
            {
                rb.drag = 0;
                AirControl.WASDHandler();
            }
        }
    }

    public void ResetDrag()
    {
        if(ps_Data.IsGrounded)
            rb.drag = Running.drag;
        else
            rb.drag = 0;
    }
}


public class PA_ArenaMovementRunning : PA_BaseMovementActionWASD, PA_ArenaBaseMovementAction
{
    float vel = 1;
    float dragTension = 0.6f;

    float slopeSlideMaxSpeed;
    float slopeSlideMaxAccel;

    private PA_ArenaMovement _arenaMovement;
    public PA_ArenaMovement ArenaMovement {get => _arenaMovement; set => _arenaMovement = value;}

    public PA_ArenaMovementRunning(PA_ArenaMovement newArenaMovement, float newSpeed, float newAccel, float newDrag, float newSlopeSpeed, float newSlopeAccel) : base(newSpeed, newAccel)
    {
        this.drag = newDrag;
        this.ArenaMovement = newArenaMovement;
        this.slopeSlideMaxAccel = newSlopeAccel;
        this.slopeSlideMaxSpeed = newSlopeSpeed;
    }

    public override void WASDHandler()
    {
        base.WASDHandler();
        

        if(ps_Data.GroundAngle > 45)
        {

        }
        else if(ps_Data.GroundAngle <= 45 && 12 < ps_Data.GroundAngle && rb.velocity.magnitude >= maxSpeed)
        {
            if(!ps_Data.IsSlopeSliding)
            {
                ps_Data.IsSlopeSliding = true;
                player.GetComponent<PC_Actions>().PlayerCollider.material = ArenaMovement.SlideMat;
            }
            vel = ps_Data.FlatVelocity;
            rb.drag = Mathf.Min(Mathf.Exp(dragTension*(0.9f*maxSpeed-vel)),1)*drag;
                
            rb.AddForce(MovementPhysics.Acceleration(slopeSlideMaxSpeed, slopeSlideMaxAccel, rb.velocity, wishDir, GetSlopeMoveDir()), ForceMode.VelocityChange);
        }
        else if(ps_Data.IsSlopeSliding)
        {
            ps_Data.IsSlopeSliding = false;
            GroundWASD();
        }
        else
        {
            GroundWASD();
        }      
    }

    public void GroundWASD()
    {
        if(ps_Data.GroundAngle <= 45)
        {
            if(ps_Data.IsNextToWall)
                player.GetComponent<PC_Actions>().PlayerCollider.material = ArenaMovement.WallMat;
            else
                player.GetComponent<PC_Actions>().PlayerCollider.material = ArenaMovement.GroundMat;
            rb.drag = drag;
            rb.AddForce(MovementPhysics.Acceleration(maxSpeed, maxAccel, rb.velocity, wishDir, GetSlopeMoveDir()), ForceMode.VelocityChange);
        }
    }
}

public class PA_ArenaMovementAirControl : PA_BaseMovementActionWASD
{
    public PA_ArenaMovementAirControl(float newSpeed, float newAccel) : base(newSpeed, newAccel)
    {
        this.drag = 0;
    }

    public override void WASDHandler()
    {
        base.WASDHandler();
        rb.AddForce(MovementPhysics.Acceleration(maxSpeed, maxAccel, rb.velocity, wishDir, wishDir), ForceMode.VelocityChange);
    }
}

public class PA_ArenaMovementJump : PA_BaseMovementAction, PA_ArenaBaseMovementAction
{
    float jumpForce;
    float pointJumpForce;
    float jumpHoldCD;
    float jumpHoldDelay;
    float jumpHoldDelayThreshold;
    float antiSpamCD;
    
    float jumpHoldCDTracker;
    float jumpHoldTracker;
    float jumpHoldDelayTracker;
    float AntiSpamTracker = 0;

    bool hasPointJumped = false;

    private PA_ArenaMovement _arenaMovement;
    public PA_ArenaMovement ArenaMovement {get => _arenaMovement; set => _arenaMovement = value;}

    public PA_ArenaMovementJump(PA_ArenaMovement newArenaMovement, float newForce, float newPointForce, float newHoldCD, float newAntiSpamCD, float newJumpHoldDelay, float newJumpHoldDelayThreshold)
    {
        this.ArenaMovement = newArenaMovement;
        this.jumpForce = newForce;
        this.pointJumpForce = newPointForce;
        this.jumpHoldCD = newHoldCD;
        this.antiSpamCD = newAntiSpamCD;
        this.jumpHoldDelay = newJumpHoldDelay;
        this.jumpHoldDelayThreshold = newJumpHoldDelayThreshold;
    }

    public void StartJump(InputAction.CallbackContext obj)
    {
        if(AntiSpamTracker <= 0)
        {
            ps_Data.IsJumping = true;
            jumpHoldTracker = jumpHoldDelayThreshold;
            jumpHoldCDTracker = 0;
            AntiSpamTracker = antiSpamCD;
            jumpHoldDelayTracker = jumpHoldDelay;

            ArenaMovement.OnFixedUpdate += OnJump;
            ArenaMovement.OnFixedUpdate += AntiSpam;
        }
    }

    public void ReleaseJump(InputAction.CallbackContext obj)
    {
        ps_Data.IsJumping = false;
        hasPointJumped = false;
        ArenaMovement.OnFixedUpdate -= OnJump;
    }

    public void Jump(float force)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up*force, ForceMode.Impulse);

        jumpHoldCDTracker = jumpHoldCD;
        jumpHoldDelayTracker = jumpHoldDelay;
    }

    public void OnJump(object sender, A_MvmtEventArg e)
    {
        if(ps_Data.IsGrounded)
        {
            if(jumpHoldTracker <= 0)
            {
                if(jumpHoldCDTracker <= 0)
                {
                    if(jumpHoldDelayTracker <= 0)
                        Jump(jumpForce);
                    else
                        jumpHoldDelayTracker -= e.DeltaTime;
                }
                else
                    jumpHoldCDTracker -= e.DeltaTime;
            }
            else if(!hasPointJumped)
            {
                Jump(pointJumpForce);
                hasPointJumped = true;
            }
        }
        else
        {
            jumpHoldCDTracker -= e.DeltaTime;
            jumpHoldTracker -= e.DeltaTime;
        }
    }

    public void AntiSpam(object sender, A_MvmtEventArg e)
    {
        if(AntiSpamTracker > 0)
            AntiSpamTracker -= e.DeltaTime;
        else
            ArenaMovement.OnFixedUpdate -= AntiSpam;
    }

    public override void SetBind(PlayerInputActions playerInputAc)
    {
        playerInputAc.Arena.Jump.performed += StartJump;
        playerInputAc.Arena.Jump.canceled += ReleaseJump;
        playerInputAc.Arena.Jump.Enable();
    }

    public override void UnsetBind(PlayerInputActions playerInputAc)
    {
        playerInputAc.Arena.Jump.performed -= StartJump;
        playerInputAc.Arena.Jump.canceled -= ReleaseJump;
        playerInputAc.Arena.Jump.Disable();
    }
}


public class PA_ArenaMovementSlide : PA_BaseMovementActionWASD, PA_ArenaBaseMovementAction
{
    public float SlideYScale;
    public Vector3 SlideScaleChange;
    float SlideXForce;
    float SlideYForce;
    float slideBoostDecay;
    float startTime = 0;

    private PA_ArenaMovement _arenaMovement;
    public PA_ArenaMovement ArenaMovement {get => _arenaMovement; set => _arenaMovement = value;}


    public PA_ArenaMovementSlide(PA_ArenaMovement newMovement, float newSpeed, float newAccel, float newYscale, float newXForce, float newYForce, float newSlideDrag, float newSlideBoostDecay) : base(newSpeed, newAccel)
    {
        this.ArenaMovement = newMovement;
        this.SlideYScale = newYscale;
        this.SlideXForce = newXForce;
        this.SlideYForce = newYForce;
        this.SlideScaleChange = new Vector3(0,1-SlideYScale,0);
        this.drag = newSlideDrag;
        this.slideBoostDecay = newSlideBoostDecay;
    }

    public void StartSlide(InputAction.CallbackContext obj)
    {
        if(!ps_Data.IsWallRiding)
        {      
            rb.drag = drag;
            ps_Data.IsSliding = true;
            float realForce = Mathf.Pow(Mathf.Min(slideBoostDecay, Time.time-startTime)/slideBoostDecay,2);
            startTime = Time.time;
            player.transform.localScale -= SlideScaleChange;
            rb.AddForce(ps_Data.wishDir * SlideXForce * realForce + Vector3.down*SlideYForce*(1/Mathf.Pow(Mathf.Clamp(ps_Data.GroundHeight, 1, 10),5)), ForceMode.Impulse);
        }
    }

    public void StopSlide(InputAction.CallbackContext obj)
    {
        if(ps_Data.IsSliding)
        {
            Vector3 stopSlideVec = ps_Data.wishDir*-SlideXForce;
            if(ps_Data.IsGrounded)
                stopSlideVec += Vector3.up*2;
            rb.AddForce(stopSlideVec, ForceMode.Impulse);
        }        
        OutStopSlide();    
    }

    public void OutStopSlide()
    {
        ArenaMovement.ResetDrag();
        ps_Data.IsSliding = false;
        player.transform.localScale = Vector3.one;
    }

    public override void WASDHandler()
    {
        base.WASDHandler();
        rb.drag = drag/5;
        rb.AddForce(MovementPhysics.Acceleration(maxSpeed, maxAccel, rb.velocity, wishDir, wishDir), ForceMode.VelocityChange);
    }

    public override void SetBind(PlayerInputActions playerInputAc)
    {
        base.SetBind(playerInputAc);
        playerInputAc.Arena.Slide.performed += StartSlide;
        playerInputAc.Arena.Slide.canceled += StopSlide;
        playerInputAc.Arena.Slide.Enable();
    }

    public override void UnsetBind(PlayerInputActions playerInputAc)
    {
        base.UnsetBind(playerInputAc);
        playerInputAc.Arena.Slide.performed -= StartSlide;
        playerInputAc.Arena.Slide.canceled -= StopSlide;
        playerInputAc.Arena.Slide.Disable();
    }
}
/*
public class PA_ArenaMovementWallRide : PA_BaseMovementActionWASD, PA_ArenaBaseMovementAction
{

}
*/
interface PA_ArenaBaseMovementAction
{
    PA_ArenaMovement ArenaMovement {get; set;}
}

public class A_MvmtEventArg : EventArgs
{
    public float DeltaTime;

    public A_MvmtEventArg()
    {
        this.DeltaTime = Time.fixedDeltaTime;
    }

    public A_MvmtEventArg(float newDeltaTime)
    {
        this.DeltaTime = newDeltaTime;
    }
}