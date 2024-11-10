using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineManager : MonoBehaviour, ILineManager
{
    private LineRenderer finalizedLineRenderer;
    private LineRenderer temporaryLineRenderer;
    private int pointIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        finalizedLineRenderer = gameObject.AddComponent<LineRenderer>();
        temporaryLineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawTemporaryLine(Vector3 prevStarPoint, Vector3 lookPoint)
    {
        temporaryLineRenderer.positionCount = 2;
        temporaryLineRenderer.SetPosition(0, prevStarPoint);
        temporaryLineRenderer.SetPosition(1, lookPoint);
        
    }

    public void ClearTemporaryLine()
    {
        temporaryLineRenderer.positionCount = 0;
    }

    public void DrawLine(Vector3 start, Vector3 end, bool isDiscontinued = false)
    {
        finalizedLineRenderer.positionCount = pointIndex + 1;
        finalizedLineRenderer.SetPosition(pointIndex, start);
        finalizedLineRenderer.positionCount = pointIndex + 1;
        finalizedLineRenderer.SetPosition(pointIndex, end);
        
        if (isDiscontinued)
        {
            // if lines should be disconnected from this point, insert same point again to make the gap
            finalizedLineRenderer.positionCount = pointIndex + 1;
            finalizedLineRenderer.SetPosition(pointIndex, end);
        }
    }
}
