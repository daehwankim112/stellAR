
using UnityEngine;



public class GazeLineManager : BaseLineManager
{
    [SerializeField]
    private LineRenderer _lineRenderer;



    public override void ClearTemporaryLine()
    {
        _lineRenderer.positionCount = 0;
    }



    public override void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false)
    {
        throw new System.NotImplementedException();
    }



    public override void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, prevStarPoint);
        _lineRenderer.SetPosition(1, lookPoint);
    }
}
