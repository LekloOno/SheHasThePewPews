using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "abex", menuName = "test/abex")]
public class abex : testpow
{
    public float testval;
    public override void PA_OnStart(InputAction.CallbackContext obj)
    {
        Debug.Log("startex" + testval);
    }
}
