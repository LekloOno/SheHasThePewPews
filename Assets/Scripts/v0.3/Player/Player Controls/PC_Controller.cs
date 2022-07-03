using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PC_Controller : MonoBehaviour
{
    public PC_Actions pcActions;
    public PC_ArenaCamera pcCamera;
    public PC_WallDetection pc_WallDetection;
    public PA_WallRide pa_WallRide;
    public PlayerInputActions playerInputActions;
    private InputAction running;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        InitiateAbilities();
        pcActions.pa_BaseMovement.InitiateMovementActions(playerInputActions);
        pcActions.ps_Data.WallRideCollisions = 0;
        pcActions.ps_Data.IsNextToWall = false;

        pcCamera.InitiateLook(playerInputActions);

        running = playerInputActions.Arena.Running;
        running.Enable();

        pa_WallRide.running = running;
        pa_WallRide.playerInputAc = playerInputActions;
        playerInputActions.Arena.Jump.performed += pa_WallRide.OnStartWallRide;
        playerInputActions.Arena.Jump.canceled += pa_WallRide.OnStopWallRide;

        playerInputActions.Arena.Reset.performed += pcActions.ResetParkour;
        playerInputActions.Arena.Reset.Enable();
    }

    private void OnDisable()
    {
        DisableAbilities();
        pcActions.pa_BaseMovement.DisableMovementActions(playerInputActions);
        pcCamera.DisableLook(playerInputActions);
        

        playerInputActions.Arena.Jump.performed -= pa_WallRide.OnStartWallRide;
        playerInputActions.Arena.Jump.canceled -= pa_WallRide.OnStopWallRide; 
        running.Disable();

        playerInputActions.Arena.Reset.performed -= pcActions.ResetParkour;
    }

    private void FixedUpdate()
    {
        
        
        //Debug.Log(running.phase);
    }

    public void InitiateAbilities()
    {
        int i = 1;
        foreach (PA_Ability pa in pcActions.pa_Abilities)
        {
            if(pa != null)
            {
                pa.setBind(playerInputActions, i);
                pa.ps_Data = pcActions.ps_Data;
                //pa.OnUseCd += pcActions.FrictionAbility_OnUseCd;
            }
            i ++;
        }
    }
    
    public void DisableAbilities()
    {
        int i = 1;
        foreach (PA_Ability pa in pcActions.pa_Abilities)
        {
            if(pa != null)
            {
                pa.unsetBind(playerInputActions, i);
            }
            i ++;
        }
    }
}
