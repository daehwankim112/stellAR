
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
        Vector3 centerPos = _centerEyeTransform.position;
        Vector3 lookDirection = _centerEyeTransform.rotation * Vector3.forward;

        IStar nearestStar = null;
        float closestAngle = _cosineAngle;

        foreach (IStar star in _stars)
        {
            if (Vector3.Dot((star.StarGameObject.transform.position - centerPos).normalized, lookDirection) > closestAngle)
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

            if (_constellation.PrevStarPosition.HasValue)
            {
                _lineManager.DrawTemporaryLine(_constellation.PrevStarPosition.Value, centerPos + (10.0f  * lookDirection));
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
            _lineManager.DrawTemporaryLine(_constellation.PrevStarPosition.Value, nearestStar.StarGameObject.transform.position);
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
