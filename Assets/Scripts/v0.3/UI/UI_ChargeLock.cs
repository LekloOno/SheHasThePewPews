using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_ChargeLock : MonoBehaviour
{
    public PC_Actions pc_Actions;
    public PS_ArenaPlayerData ps_Data;
    public Image ChargeLockBar;
    float lastChargeLock;
    float chargeLockTracker;

    public event EventHandler OnUIUpdate;

    void Start()
    {
        //pc_Actions.OnNewChargeLock += Action_OnNewChargeLock;
        foreach(PA_FrictionAbility pa in pc_Actions.pa_Abilities)
        {
            pa.OnChargeLockSet += Ability_OnChargeLockSet;
        }
    }

    void FixedUpdate()
    {
        OnUIUpdate?.Invoke(this, EventArgs.Empty);
    }

    void Ability_OnChargeLockSet(object sender, FAbility_UseCDEventArg e)
    {
        lastChargeLock = e.ChargeLock;
        OnUIUpdate -= UI_OnChargeLockActive;
        OnUIUpdate += UI_OnChargeLockActive;
    }

    void UI_OnChargeLockActive(object sender, EventArgs e)
    {
        if(ps_Data.ChargeLockTracker > 0)
            ChargeLockBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(0, 40, ps_Data.ChargeLockTracker/lastChargeLock),1.3f);
        else
            OnUIUpdate -= UI_OnChargeLockActive;
    }
}
