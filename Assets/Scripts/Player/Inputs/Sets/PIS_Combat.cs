using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PIS_Combat : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    public Vector3 WishDir;
    public Vector2 RunningAxis;
    public float LastBackward = Mathf.NegativeInfinity;
    public PSM_Combat MovementState;

    public event EventHandler OnInputActionsEnable;
    public event EventHandler OnInputActionsDisable;

    [SerializeField] Transform playerDir;
    
    bool Started = false;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void Start()
    {
        OnInputActionsEnable?.Invoke(this, EventArgs.Empty);
    }

    void OnEnable()
    {
        playerInputActions.Arena.Enable();
        if(Started)
            OnInputActionsEnable?.Invoke(this, EventArgs.Empty);
        else
            Started = true;
    }

    void OnDisable()
    {
        playerInputActions.Arena.Disable();
        OnInputActionsDisable?.Invoke(this, EventArgs.Empty);
    }

    void FixedUpdate()
    {
        RunningAxis = playerInputActions.Arena.Running.ReadValue<Vector2>();
        if(RunningAxis.y < 0)
        {
            LastBackward = Time.time;
        }
        WishDir = playerDir.forward * RunningAxis.y + playerDir.right * RunningAxis.x;
    }
}
