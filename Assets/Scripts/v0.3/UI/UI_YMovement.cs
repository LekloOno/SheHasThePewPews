using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_YMovement : MonoBehaviour
{/*
    public PC_Actions pc_Actions;
    public PS_ArenaPlayerData ps_Data;
    public GameObject UI_Elements;
    
    public float landingAnimDuration;
    
    float animTrack;
    float ySpeed;

    public event EventHandler OnUpdate;

    void OnEnable()
    {
        pc_Actions.OnLanding += UI_OnLand;
    }

    void OnDisable()
    {
        pc_Actions.OnLanding -= UI_OnLand;
    }

    void Update()
    {
        OnUpdate?.Invoke(this, EventArgs.Empty);   
    }

    void UI_OnLand(object sender, EventArgs e)
    {
        OnUpdate -= NormalY;
        animTrack = landingAnimDuration;
        OnUpdate += Landing;
    }

    void Landing(object sender, EventArgs e)
    {
        if(animTrack>0)
        {
            animTrack += Time.deltaTime;
            if(animTrack>landingAnimDuration/2)
            {
                foreach(GameObject g in UI_Elements)
                {

                }
            }
        }
        else
        {
            OnUpdate -= Landing;
            OnUpdate += NormalY;
        }
    }

    void NormalY(object sender, EventArgs e)
    {
        ySpeed = ps_Data.velocity.y;

    }*/
}
