
using System.Collections.Generic;
using UnityEngine;

public class GazeManagerTestConstellation : MonoBehaviour, IConstellation
{
    public GameObject StarPrefab;
    public GameObject GazeManager;
    public Vector3? PrevStarPosition { get; private set; }

    public StarGroup StarGroup;

    public float Scale;
    public float Rotation;
    public float Radius;
    public Transform Center;


    public readonly List<GameObject> Stars = new();


    void Start()
    {
        StarGroup = new(Radius);

        SpawnRandomStars(20);
    }



    void Update()
    {
        StarGroup.Radius = Radius;
        StarGroup.ReCenter(Center.rotation * Vector3.forward);
        List<Vector3> scaledStars = StarGroup.ScaledRotatedTranslated(Scale, Rotation * (Time.time + Mathf.Sin(Time.time * 0.1f)), Center.position + 2.0f * Vector3.up);

        for (int i = 0; i < Stars.Count; i++)
        {
            Stars[i].transform.position = scaledStars[i];
        }
    }



    private void SpawnRandomStars(int num)
    {
        List<IStar> starComponents = new();
        for (int _ = 0; _ < num; _++)
        {
            GameObject newStar = Instantiate(StarPrefab);
            Vector2 angles = new(Random.Range(10.0f, 18.0f), Random.Range(-40.0f, 10.0f));
            Vector3 pos = StarAligner.TransformAngles(angles);
            newStar.transform.position = Radius * pos;
            StarGroup.StarPositions.Add(pos);
            Stars.Add(newStar);
            starComponents.Add(newStar.GetComponent<IStar>());
        }

        GazeManager gazeManagerComponent = GazeManager.GetComponent<GazeManager>();
        gazeManagerComponent.GiveStarList(starComponents.ToArray(), this);
    }



    public void Build(List<(IStar, IStar)> stars)
    {
        throw new System.NotImplementedException();
    }



    public void Selected(IStar star)
    {
        star.Confirmed();
        PrevStarPosition = star.Position;
    }
}
