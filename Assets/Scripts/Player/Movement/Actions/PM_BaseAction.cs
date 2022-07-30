using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_BaseAction<T> : MonoBehaviour where T : DATA_BaseAction
{
    [Header("Common")]
    [SerializeField] protected T data;
    public T Data
    {
        get => data;
    }
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PIS_Combat inputHandler;
    [SerializeField] protected PSS_Ground spaceState;
}
