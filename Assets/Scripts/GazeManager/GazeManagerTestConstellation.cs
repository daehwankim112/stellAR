
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
        SpawnRandomStars(10);
    }



    private void SpawnRandomStars(int num)
    {
        List<IStar> starComponents = new();
        for (int _ = 0; _ < num; _++)
        {
            GameObject newStar = Instantiate(StarPrefab);
            newStar.transform.position = 5.0f * Random.insideUnitSphere;
            Stars.Add(newStar);
            starComponents.Add(newStar.GetComponent<IStar>());
        }

        GazeManager gazeManagerComponent = GazeManager.GetComponent<GazeManager>();
        gazeManagerComponent.GiveStarList(starComponents.ToArray());
        gazeManagerComponent.Constellation = this;
    }



    public void Build((IStar, IStar)[] stars)
    {
        throw new System.NotImplementedException();
    }



    public void LookingAt(IStar star)
    {
        star.LookingAt();
    }



    public void Selected(IStar star)
    {
        star.Confirmed();
    }
}
