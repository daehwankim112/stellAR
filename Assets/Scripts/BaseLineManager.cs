using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLineManager : MonoBehaviour, ILineManager
{
    public abstract void ClearTemporaryLine();
    public abstract void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false);
    public abstract void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint);
}
