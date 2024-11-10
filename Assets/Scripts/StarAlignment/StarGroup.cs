
using System.Collections.Generic;
using UnityEngine;



public class StarGroup
{
    /// <summary>
    /// Spherical coords
    /// </summary>
    public readonly List<Vector3> StarPositions;

    public float Radius;


    public StarGroup(List<Vector3> positions, float radius)
    {
        StarPositions = positions;
        Radius = radius;
    }


    public void Scale(float factor)
    {
        Vector3 com = CenterOfMassSurface();

        for (int posIndex = 0; posIndex < StarPositions.Count; posIndex++)
        {
            Vector3 pos = StarPositions[posIndex];
            float distance = Vector3.Angle(com, pos);
            float scaledDistance = distance * factor;
            Vector3 poleVector = Vector3.Cross(com, pos).normalized;
            Quaternion travel = Quaternion.AngleAxis(scaledDistance, poleVector);
            Vector3 scaledPos = travel * com;
            StarPositions[posIndex] = scaledPos;
        }
    }



    public Vector3 CenterOfMassSurface()
    {
        Vector3 sum = Vector3.zero;
        StarPositions.ForEach(p => sum += p);
        return (sum / StarPositions.Count).normalized;
    }
}
