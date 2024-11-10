
using UnityEngine;



public class LineManager : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    private Vector3 _prevPosition;

    [SerializeField]
    private float _slerpSpeed;



    public void ClearTemporaryLine()
    {
        _lineRenderer.positionCount = 0;
    }



    public void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false)
    {
        throw new System.NotImplementedException();
    }



    public void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint)
    {
        if (_lineRenderer.positionCount == 0)
        {
            _lineRenderer.positionCount = 2;
            _prevPosition = lookPoint;
        }

        lookPoint = Vector3.Lerp(_prevPosition, lookPoint, _slerpSpeed);
        _prevPosition = lookPoint;

        _lineRenderer.SetPosition(0, prevStarPoint);
        _lineRenderer.SetPosition(1, lookPoint);
    }
}
