using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public GameObject star;
    public GameObject parent;
    public ConstellationScriptableObject[] constellationManager;
    public int constellationNumber = 1;
    public Constellation constellation = new();
    public List<(IStar, IStar)> starTupleList = new();
    
    [SerializeField] List<Star> starList = new List<Star>();
    [SerializeField] GazeManager gazeManager;
    
    void Start() {
        // Pick constellation randomly from ConstellationList
        int randomIndex = Random.Range(0, constellationManager.Length); // Get random index
        // ConstellationScriptableObject constellationData = constellationManager[randomIndex];
        
        // Have starting instructions for person to get them into position
        SpawnStars(0);
        // constellation.Build();
        
    }

    void Update() {
        //if (constellation is complete) {
        //  constellationNumber++;
        //  SpawnStars();
        //}
    }

    private void SpawnStars(int index) {
        // for (int i = 0; i < constellationManager[constellationNumber].starLocations.Length; i++)
        // {
        //     GameObject currentEntity = Instantiate(star);
        //     currentEntity.transform.position = constellationManager[constellationNumber].starLocations[i];
        //     currentEntity.transform.SetParent(parent.transform);
        //     currentEntity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        // }
        
        starList.Clear();
        
        Debug.Log("star location length: " + constellationManager[index].starLocations.Length);

        for (int i = 0; i < constellationManager[index].starLocations.Length; i++)
        {
            var _instantiatedStar = Instantiate(star, constellationManager[index].starLocations[i], Quaternion.identity);
            starList.Add(_instantiatedStar.GetComponent<Star>());
        }
        
        
        for (int i = 0; i < constellationManager[index].startIndices.Length; i++)
        {
            int startIndex = constellationManager[index].startIndices[i];
            int endIndex = constellationManager[index].endIndices[i];
            
            Star startStar = starList[startIndex];
            Star endStar = starList[endIndex];
            if (startStar != null && endStar != null)
            {
                startStar.Position = constellationManager[index].starLocations[startIndex];
                endStar.Position = constellationManager[index].starLocations[endIndex];
            }

            (IStar, IStar) starTuple = (startStar as IStar, endStar as IStar);
            
            starTupleList.Add(starTuple);
        }

        constellation.Build(starTupleList);
        gazeManager.GiveStarList(starList.ToArray(), constellation);
        
    }
}
