
using System.Collections.Generic;
using UnityEngine;

public class GazeManagerTestConstellation : MonoBehaviour, IConstellation
{
    public GameObject StarPrefab;
    public GameObject GazeManager;
    public Vector3? PrevStarPosition { get; private set; }


    public readonly List<GameObject> Stars = new();


    void Start()
    {
        SpawnRandomStars(20);
    }



    private void SpawnRandomStars(int num)
    {
        List<IStar> starComponents = new();
        for (int _ = 0; _ < num; _++)
        {
            GameObject newStar = Instantiate(StarPrefab);
            Vector2 angles = new(Random.Range(0.0f, 24.0f), Random.Range(-90.0f, 90.0f));
            newStar.transform.position = 5.0f * StarAligner.TransformAngles(angles);
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
