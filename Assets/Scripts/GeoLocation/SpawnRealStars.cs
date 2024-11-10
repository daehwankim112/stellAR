
using System.Collections.Generic;
using UnityEngine;



public class SpawnRealStars : MonoBehaviour
{
    public GameObject starPrefab;

    public float sizeMultiplier;



    void Start()
    {
        List<Vector3> starAngles = StarAligner.LoadStars("Assets/Data/realStars.tsv");

        for (int star = 0; star < starAngles.Count; star++)
        {
            Vector3 starPos = StarAligner.TransformAngles(new(starAngles[star].x, starAngles[star].y));
            float magnitude = starAngles[star].z;
            GameObject newStar = Instantiate(starPrefab, transform);
            newStar.transform.position = 100.0f * starPos;
            newStar.transform.localScale = sizeMultiplier * Mathf.Pow(10.0f, magnitude / -5.0f) * Vector3.one;
        }
    }
}
