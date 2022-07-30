using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Start : MonoBehaviour
{
    [SerializeField] PIS_Combat inputHandler;
    void Start()
    {
        inputHandler.enabled = true;
    }
}
