using System.Collections;
using System.Collections.Generic;using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;

public interface IConstellation
{
    public void LookAt(IStar star);
    public void Build((IStar, IStar)[] stars);
}
