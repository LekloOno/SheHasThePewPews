using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Drift : MonoBehaviour
{
    [SerializeField] PMA_DriftControl driftHandler;
    [SerializeField] PSS_Ground spaceState;
    [SerializeField] Image bar;

    float k;

    void Update()
    {
        if(driftHandler.enabled && spaceState.IsGrounded)
        {
            k = (driftHandler.Duration - driftHandler.SlideTime)/driftHandler.Duration;
        }
        else
        {
            k = 0;
        }
        bar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(0,100,k),bar.rectTransform.sizeDelta.y);
    }
}
