
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public static class StarAligner
{
    /// <summary>
    /// Transform a list of angles (RA: hours, Dec: degrees) to usable coordinates for the app
    /// </summary>
    /// <param name="angles">
    /// A list of Vector2 where the x is Right Ascension (angular hours) and y is Declination (degrees)
    /// </param>
    /// <returns>
    /// A list of Vector3 coordinates that are usable
    /// </returns>
    public static List<Vector3> Align(List<Vector2> angles) => (List<Vector3>)angles.Select(TransformAngles);



    // Magic numbers are just hours and degrees to radians
    public static Vector2 ToRadians(Vector2 angles) => new(-angles.x * 0.261799387799f, (90.0f - angles.y) * 0.0174532925199f);



    public static Vector3 SphereToCartesian(Vector2 angles)
        => new(Mathf.Sin(angles.y) * Mathf.Cos(angles.x),
               Mathf.Sin(angles.y) * Mathf.Sin(angles.x),
               Mathf.Cos(angles.y));


    public static Vector3 TransformAngles(Vector2 angles) => SphereToCartesian(ToRadians(angles));
}
