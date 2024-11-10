
using System.Collections.Generic;
using UnityEngine;



public class GazeManager : MonoBehaviour, IGazeManager
{
    private readonly List<IStar> _stars = new();

    [SerializeField]
    private float _cosineAngle;

    [SerializeField]
    private float _selectTime;

    [SerializeField]
    private Transform _centerEyeTransform;

    [SerializeField]
    private GazeLineManager _lineManager;

    private IConstellation _constellation;

    private IStar _prevLookAt;

    public bool GazeOn = false;

    public float LookAngle
    {
        get
        {
            return Mathf.Acos(_cosineAngle);
        }
        set
        {
            _cosineAngle = Mathf.Cos(value);
        }
    }

    public float Timer = 0.0f;



    void LateUpdate()
    {
        if (!GazeOn) return;
        
        Vector3 centerPos = _centerEyeTransform.position;
        Vector3 lookDirection = _centerEyeTransform.rotation * Vector3.forward;

        IStar nearestStar = null;
        float closestAngle = _cosineAngle;

        foreach (IStar star in _stars)
        {
            if (star == null) continue;
            
            if (Vector3.Dot((star.Position - centerPos).normalized, lookDirection) > closestAngle)
            {
                nearestStar = star;
            }
        }

        if (nearestStar == null)
        {
            // No star selected
            Timer = 0.0f;
            _prevLookAt?.NotLookingAt();
            _prevLookAt = null;

            if (_constellation != null && _constellation.PrevStarPosition != null)
            {
                float lineDistance = (_constellation.PrevStarPosition.Value - centerPos).magnitude;
                _lineManager.DrawTemporaryLine(_constellation.PrevStarPosition.Value, centerPos + (lineDistance * lookDirection));
            }
            else
            {
                _lineManager.ClearTemporaryLine();
            }

            return;
        }

        // Switch selected star if necessary
        if (_prevLookAt == null || nearestStar != _prevLookAt)
        {
            Timer = 0.0f;
            _prevLookAt?.NotLookingAt();
            _prevLookAt = nearestStar;
        }

        // Select star
        if (Timer >= _selectTime)
        {
            Timer = 0.0f;
            _constellation.Selected(nearestStar);
        }
        else
        {
            Timer += Time.deltaTime;
            nearestStar.LookingAt();
        }

        if (_constellation.PrevStarPosition.HasValue)
        {
            _lineManager.DrawTemporaryLine(_constellation.PrevStarPosition.Value, nearestStar.Position);
        }
        else
        {
            _lineManager.ClearTemporaryLine();
        }
    }



    public void GiveStarList(IStar[] stars, IConstellation constellation)
    {
        _stars.Clear();
        _stars.AddRange(stars);
        _constellation = constellation;
    }
}
