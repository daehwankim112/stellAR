
using System.Collections.Generic;
using UnityEngine;



public class StarGroup
{
    /// <summary>
    /// Spherical coords
    /// </summary>
    public readonly List<Vector3> StarPositions;

    public float Radius;

    public float Spread
    {
        get
        {
            Vector3 com = CenterOfMassSurface();

            float angleSum = 0.0f;
            StarPositions.ForEach(p => angleSum += Vector3.Angle(com, p));
            return Radius * angleSum / StarPositions.Count;
        }
    }



    public StarGroup(float radius = 1.0f)
    {
        StarPositions = new();
        Radius = radius;
    }



    public StarGroup(List<Vector3> positions, float radius = 1.0f)
    {
        StarPositions = positions;
        StarPositions.ForEach(p => p = p.normalized);
        Radius = radius;
    }


    public void ReCenter(Vector3 center)
    {
        center = center.normalized;
        Vector3 com = CenterOfMassSurface();
        if ((center - com).sqrMagnitude < 0.01f) return;

        Vector3 poleVector = Vector3.Cross(com, center).normalized;
        float distance = Vector3.Angle(com, center);
        Quaternion travel = Quaternion.AngleAxis(distance, poleVector);

        for (int posIndex = 0; posIndex < StarPositions.Count; posIndex++)
        {
            Vector3 rotatedPos = travel * StarPositions[posIndex];
            StarPositions[posIndex] = rotatedPos;
        }
    }



    /// <summary>
    /// Dark magic (mathematics). I did this at 4:30 in the morning don't ask too many questions please.
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="angle"></param>
    /// <param name="translation"></param>
    /// <returns></returns>
    public List<Vector3> ScaledRotatedTranslated(float scale, float angle, Vector3 translation)
    {
        List<Vector3> transformedPositions = new();

        Vector3 com = CenterOfMassSurface();

        Quaternion rotation = Quaternion.AngleAxis(angle, com);

        for (int posIndex = 0; posIndex < StarPositions.Count; posIndex++)
        {
            Vector3 pos = StarPositions[posIndex];
            float distance = Vector3.Angle(com, pos);
            float scaledDistance = distance * scale;
            Vector3 poleVector = Vector3.Cross(com, pos).normalized;
            Quaternion travel = Quaternion.AngleAxis(scaledDistance, poleVector);
            Vector3 scaledPos = rotation * travel * com;
            transformedPositions.Add(translation + (Radius * scaledPos));
        }

        return transformedPositions;
    }



    public Vector3 CenterOfMassSurface()
    {
        Vector3 sum = Vector3.zero;
        StarPositions.ForEach(p => sum += p);
        return (sum / StarPositions.Count).normalized;
    }
}
