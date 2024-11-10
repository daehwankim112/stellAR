using System.Collections;
using System.Collections.Generic;using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;

public interface IConstellation
{
    public void Selected(IStar star);
    public void Build(List<(IStar, IStar)> stars);
    public Vector3? PrevStarPosition { get; }
    
}
