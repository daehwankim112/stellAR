
using System.Collections.Generic;
using UnityEngine;



public class GazeManager : MonoBehaviour, IGazeManager
{
    private readonly List<StarData> _stars = new();

    [SerializeField]
    private float _cosineAngle;

    [SerializeField]
    private float _selectTime;

    [SerializeField]
    private Transform _centerEyeTransform;

    public IConstellation Constellation;

    private StarData _prevLookAt;

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

        foreach (StarData star in _stars)
        {
            if (Vector3.Dot((star.Position - centerPos).normalized, lookDirection) > _cosineAngle)
            {
                // Constellation.LookingAt(star.Star);
                Timer += Time.deltaTime;

                // Switch selected star if necessary
                if (_prevLookAt == null || star != _prevLookAt)
                {
                    Timer = 0.0f;
                    _prevLookAt = star;
                }

                // Select star
                if (Timer >= _selectTime)
                {
                    Constellation.Selected(star.Star);
                    Timer = 0.0f;
                }
                return;
            }
        }
        
        // No star selected
        Timer = 0.0f;
        _prevLookAt = null;
    }



    /// <summary>
    /// Give a list of stars to look at in the scene
    /// </summary>
    /// <param name="stars">
    /// The stars
    /// </param>
    public void GiveStarList(IStar[] stars)
    {
        _stars.Clear();
        
        foreach (IStar star in stars)
        {
            if (star is MonoBehaviour starMB)
            {
                if (starMB.TryGetComponent(out Transform transform))
                {
                    _stars.Add(new(star, transform.position));
                }
            }
        }
    }



    /// <summary>
    /// Utility class to get around the IStar interface
    /// </summary>
    private class StarData
    {
        public IStar Star;
        public Vector3 Position;

        public StarData(IStar star, Vector3 position)
        {
            Star = star;
            Position = position;
        }
    }

    public void GiveStarList(IStar[] stars, IConstellation constellation)
    {
        
    }
}
