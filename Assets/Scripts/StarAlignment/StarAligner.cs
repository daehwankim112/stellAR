
using System.Collections.Generic;
using System.IO;
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



    /// <summary>
    /// Loads star data from a file and converts it to a list of Vector3 (RA, Dec, magnitude).
    /// </summary>
    /// <param name="path">The path to the file containing star data.</param>
    /// <returns>A list of Vector3 coordinates representing the stars.</returns>
    public static List<Vector3> LoadStars(string path)
    {
        using FileStream fs = File.OpenRead(path);
        using StreamReader reader = new(fs);

        List<Vector3> stars = new();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split('\t');
            if (parts.Length == 8 &&
                int.TryParse(parts[0], out int ra_hours) &&
                int.TryParse(parts[1], out int ra_minutes) &&
                float.TryParse(parts[2], out float ra_seconds) &&
                (parts[3] == "+" || parts[3] == "-") &&
                int.TryParse(parts[4], out int dec_degrees) &&
                int.TryParse(parts[5], out int dec_minutes) &&
                int.TryParse(parts[6], out int dec_seconds) &&
                float.TryParse(parts[7], out float magnitude))
            {
                float ra = ra_hours + ((ra_minutes / 60.0f) + (ra_seconds / 3600.0f)) * 15.0f;
                float dec = (parts[3] == "-" ? -1 : 1) * (dec_degrees + (dec_minutes / 60.0f) + (dec_seconds / 3600.0f));
                stars.Add(new(ra, dec, magnitude));
            }
        }

        return stars;
    }



    /// <summary>
    /// Converts angles from hours and degrees to radians.
    /// </summary>
    /// <param name="angles">A Vector2 where x is Right Ascension (hours) and y is Declination (degrees).</param>
    /// <returns>A Vector2 where x and y are in radians.</returns>
    public static Vector2 ToRadians(Vector2 angles) => new(-angles.x * 0.261799387799f, (90.0f - angles.y) * 0.0174532925199f);



    public static Vector3 SphereToCartesian(Vector2 angles)
        => new(Mathf.Sin(angles.y) * Mathf.Cos(angles.x),
               Mathf.Sin(angles.y) * Mathf.Sin(angles.x),
               Mathf.Cos(angles.y));



    public static Vector3 TransformAngles(Vector2 angles) => SphereToCartesian(ToRadians(angles));



    public static StarGroup StarGroupFromRaDec(List<Vector2> angles, float radius = 1.0f) => new(angles.Select(TransformAngles).ToList(), radius);
}
