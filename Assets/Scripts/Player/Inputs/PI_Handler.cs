using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Handler : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    public PSM_Combat MovementState;
    

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

    }

    void OnEnable()
    {
        playerInputActions.Arena.Running.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Arena.Running.Disable();
    }

    
}
