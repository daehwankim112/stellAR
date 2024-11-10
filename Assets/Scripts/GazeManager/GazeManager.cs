
using System.Collections.Generic;
using UnityEngine;



public class GazeManager : MonoBehaviour, IGazeManager
{
    private readonly List<Vector3> _starPositions = new();
    private readonly List<IStar> _stars = new();

    [SerializeField]
    private float _cosineAngle;
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

    [SerializeField]
    private Transform _centerEyeTransform;

    public IConstellation Constellation;



    void Update()
    {
        Vector3 centerPos = _centerEyeTransform.position;
        Vector3 lookDirection = _centerEyeTransform.rotation * Vector3.forward;

        for (int starIndex = 0; starIndex < _stars.Count; starIndex++)
        {
            if (Vector3.Dot((_starPositions[starIndex] - centerPos).normalized, lookDirection) < _cosineAngle)
            {
                Constellation.LookAt(_stars[starIndex]);
            }
        }
    }



    public void GiveStarList(IStar[] stars)
    {
        _starPositions.Clear();
        _stars.Clear();
        
        foreach (IStar star in stars)
        {
            if (star is MonoBehaviour starMB)
            {
                if (starMB.TryGetComponent(out Transform transform))
                {
                    _starPositions.Add(transform.position);
                    _stars.Add(star);
                }
            }
        }
    }
}
