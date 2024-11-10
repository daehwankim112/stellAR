
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public IConstellation Constellation;

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
            Constellation.Selected(nearestStar);
        }
        else
        {
            Timer += Time.deltaTime;
            nearestStar.LookingAt();
        }
    }



    public void GiveStarList(IStar[] stars, IConstellation constellation)
    {
        _stars.Clear();
        _stars.AddRange(stars);
    }
}
