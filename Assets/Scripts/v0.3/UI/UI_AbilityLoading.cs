using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilityLoading : MonoBehaviour
{
    [SerializeField] int abilitySlot;
    [SerializeField] PS_ArenaPlayerData ps_Data;
    [SerializeField] PC_Actions pc_Actions;

    [SerializeField] Image abilityCharge;
    [SerializeField] Image abilityChargeBG;
    [SerializeField] Image abilityCD_overlap;
    [SerializeField] Image abilityCD_ring;

    [SerializeField] GameObject stackPrefab;
    [SerializeField] GameObject[] stackInstances;
    public float stackDotsSpace;
    
    [SerializeField] PA_FrictionAbility ability;
    PA_FrictionCharge friction;
    PA_TimeCooldown cd;
    float t;
    Vector2 newWidth;

    bool cdLoading;
    public float stackLoadAnimLength;
    float stackLoadAnimLeft;
    public float cdResetAnimLength;
    float cdResetAnimLeft;

    public Color stackDotColor;

    public Color minChargeColor;
    public Color maxChargeColor;
    public Color fullChargeColor;

    public Color onCooldown;

    // Start is called before the first frame update
    void Start()
    {
        RefAbility();

        friction.OnStackLoad += Friction_OnStackLoad;
        ability.OnUseCd += Ability_OnUseCd;
        ability.OnTCDreset += Ability_OnTCDreset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCooldown();
        UpdateFrictionCharge();
        if(!cdLoading)
            CooldownReset();
        if(stackLoadAnimLeft > 0)
            StackLoadAnim();
    }

    void UpdateCooldown()
    {  
        if(cdLoading)
        {
            if(abilityCharge.fillAmount != 0)
                abilityCD_overlap.fillAmount = abilityCharge.fillAmount;
            else
                abilityCD_overlap.fillAmount = 1;
            abilityCD_ring.fillAmount = 1-cd.chargeTracker/cd.baseCharge;
            //abilityCD_overlap.color = Color.Lerp(onCooldown, Color.clear, Mathf.Pow(cd.chargeTracker/cd.baseCharge,1));
        }
    }

    void CooldownReset()
    {
        if(cdResetAnimLeft > 0)
        {
            abilityCD_overlap.color = Color.Lerp(Color.clear, onCooldown, cdResetAnimLeft/cdResetAnimLength);
            cdResetAnimLeft -= Time.fixedDeltaTime;
        }
        else
            abilityCD_overlap.color = Color.clear;
    }

    void StackLoadAnim()
    {
        newWidth = Vector2.one*Mathf.Lerp(1.5f, 1, Mathf.Pow((2*Mathf.Abs(stackLoadAnimLeft-stackLoadAnimLength/2))/stackLoadAnimLength,2))*25;
        abilityCharge.rectTransform.sizeDelta = newWidth;
        abilityChargeBG.rectTransform.sizeDelta = newWidth;
        abilityCD_overlap.rectTransform.sizeDelta = newWidth;
        abilityCD_ring.rectTransform.sizeDelta = newWidth;
        stackLoadAnimLeft -= Time.fixedDeltaTime;
    }

    void UpdateFrictionCharge()
    {
        if(friction.chargeTracker < friction.fullCharge)
        {
            float stackProgress = friction.StackForChargeToFloat(friction.chargeTracker) % 1;
            abilityCharge.fillAmount = stackProgress;
            abilityCharge.color = Color.Lerp(minChargeColor,maxChargeColor,stackProgress);
        }
        else
        {
            abilityCharge.fillAmount = 1;
            abilityCharge.color = fullChargeColor;
        }
    }

    void Friction_OnStackLoad(object sender, EventArgs e)
    {
        stackInstances[friction.stacksTracker].GetComponent<Image>().color = stackDotColor;
        stackLoadAnimLeft = stackLoadAnimLength;
    }

    void Ability_OnUseCd(object sender, EventArgs e)
    {
        abilityCD_overlap.fillAmount = abilityCharge.fillAmount;
        abilityCD_overlap.color = onCooldown;
        cdLoading = true;
        for(int i = 0; i<friction.stacksTracker; i++)
        {
            if(i == friction.stacksTracker-1)
                stackInstances[i].GetComponent<Image>().color = Color.clear;
            else
                stackInstances[i].GetComponent<Image>().color = onCooldown;
        }
    }

    void Ability_OnTCDreset(object sender, EventArgs e)
    {
        cdLoading = false;
        cdResetAnimLeft = cdResetAnimLength;
        abilityCD_ring.fillAmount = 0;
        for(int i = 0; i<friction.stacksTracker; i++)
        {
            stackInstances[i].GetComponent<Image>().color = stackDotColor;
        }
    }

    public void RefAbility()
    {
        ability = pc_Actions.pa_Abilities[abilitySlot];
        stackInstances = new GameObject[ability.FrictionCharge.stacks];
        friction = ability.FrictionCharge;
        cd = ability.TimeCooldown;
        GenerateUIstacks();
    }

    public void GenerateUIstacks()
    {
        stackDotsSpace = 2*stackDotsSpace/ability.FrictionCharge.stacks;
        GameObject stacksHolder = Instantiate(new GameObject("Charge Stacks"), transform);
        for(int i = 0; i<ability.FrictionCharge.stacks; i++)
        {
            stackInstances[i] = Instantiate(stackPrefab);
            stackInstances[i].GetComponent<Image>().color = Color.clear;
            stackInstances[i].transform.SetParent(stacksHolder.transform, false);
            stackInstances[i].transform.localPosition += new Vector3(-((ability.FrictionCharge.stacks-1)*stackDotsSpace)/2+i*stackDotsSpace, -20, 0);
            stackInstances[i].name = "ChargeStack_" + (i+1).ToString();
        }
    }
}
