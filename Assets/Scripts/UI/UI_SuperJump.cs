using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SuperJump : MonoBehaviour
{
    [SerializeField] PMA_SuperJump _superjumpHandler;
    [SerializeField] PSS_Ground spaceState;
    [SerializeField] Image bar;

    float k;

    void Update()
    {
        if(_superjumpHandler.enabled && spaceState.IsGrounded)
        {
            k = Mathf.Min(_superjumpHandler.Tracker_superJumpCharge, _superjumpHandler.FullChargeTime)/_superjumpHandler.FullChargeTime;
        }
        else
        {
            k = 0;
        }
        bar.fillAmount = k;
    }
}
