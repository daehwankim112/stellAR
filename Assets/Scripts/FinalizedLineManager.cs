
using UnityEngine;



public class FinalizedLineManager : BaseLineManager
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    private Vector3 _prevPosition;

    [SerializeField]
    private float _slerpSpeed;

    private int pointIndex = 0;
    

    public override void ClearTemporaryLine()
    {
        throw new System.NotImplementedException();
    }
    
    public override void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false)
    {
        // finalizedLineRenderer.positionCount = pointIndex + 1;
        // finalizedLineRenderer.SetPosition(pointIndex, start);
        // finalizedLineRenderer.positionCount = pointIndex + 1;
        // finalizedLineRenderer.SetPosition(pointIndex, end);
        //
        // if (isDiscontinued)
        // {
        //     // if lines should be disconnected from this point, insert same point again to make the gap
        //     finalizedLineRenderer.positionCount = pointIndex + 1;
        //     finalizedLineRenderer.SetPosition(pointIndex, end);
        // }
        // throw new System.NotImplementedException();
        _lineRenderer.positionCount = pointIndex + 1;
        _lineRenderer.SetPosition(pointIndex, start);
        _lineRenderer.positionCount = pointIndex + 1;
        _lineRenderer.SetPosition(pointIndex, end);
        
        // if (isDiscontinued)
        // {
        //     // if lines should be disconnected from this point, insert same point again to make the gap
        //     _lineRenderer.positionCount = pointIndex + 1;
        //     _lineRenderer.SetPosition(pointIndex, end);
        // }
        
    }

    public override void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint)
    {
        throw new System.NotImplementedException();
        // if (_lineRenderer.positionCount == 0)
        // {
        //     _lineRenderer.positionCount = 2;
        //     _prevPosition = lookPoint;
        // }
        //
        // lookPoint = Vector3.Lerp(_prevPosition, lookPoint, _slerpSpeed);
        // _prevPosition = lookPoint;
        //
        // _lineRenderer.SetPosition(0, prevStarPoint);
        // _lineRenderer.SetPosition(1, lookPoint);
    }
}
