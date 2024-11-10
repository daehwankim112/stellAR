
using UnityEngine;



public interface ILineManager
{
    public void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint);
    public void ClearTemporaryLine();
    public void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false);
}
