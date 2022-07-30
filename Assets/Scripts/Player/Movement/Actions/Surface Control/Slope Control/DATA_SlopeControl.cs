using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATA_SlopeControl", menuName = "Player Movement/Actions/Slope Control Data")]
public class DATA_SlopeControl : DATA_SurfaceControl
{
    public float EnteringSpeed;
    public float FullSpeed;
    public float DragTension;
    public PhysicMaterial Mat;
}
