using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_Jump", menuName = "Player Movement/Actions/Jump Data")]
public class DATA_Jump : DATA_BaseAction
{
    [Header("Strength")]
    public float HeldJumpForce = 3.8f;      //The force value used for a Held jump.
    public float TapJumpForce = 4;          //The force value used for a Tap jump.
    public float PreYMulltiplier = 0;       //Between 0 and 1 most likely. 0 means the Y velocity is completely reseted before applying the jump force, 1 means the jump only applies its jump force, no matter the context.
    //It also means that with the same force, a jump will always propulse the player of the same height if PreYMultiplier is set to 0.

    [Header("Holding")]
    public float HeldJumpCD = 0.3f;         //The cooldown between two jump when the jump bind is held down.
    public float HeldJumpDelay = 0.02f;     //The delay between the moment you land and the moment you jump if the jump is considered held.
    public float HeldJumpThreshold = 0.07f; //If you held the jump bind down since at least this much time, the jump will be considered held.

    [Header("Decay")]
    public float JumpDecayRecover = 0;      //The time required between two jump to get full jump strength.
    public float JumpDecayStrength = 1;     //The tension of the decay curve. Basically the power at which it will be raised. >1 means it gets closer to the full force exponentially, <1 means it has less impact.
}
