
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour, IGazeManager
{
    private readonly List<Vector3> _starPositions = new();



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GiveStarList(IStar[] stars)
    {
        _starPositions.Clear();
        
        foreach (IStar star in stars)
        {
            if (star is MonoBehaviour starMB)
            {
                if (starMB.TryGetComponent(out Transform transform))
                {
                    _starPositions.Add(transform.position);
                }
            }
        }
    }
}
