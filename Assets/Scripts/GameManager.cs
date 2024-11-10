using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject star;
    public GameObject parent;
    public ConstellationScriptableObject[] constellationManager = new ConstellationScriptableObject[6];
    public int constellationNumber = 1;
    public Constellation constellation;
    public List<(IStar, IStar)> starTupleList;
    
    private List<IStar> starList = new List<IStar>();

    void Start() {
        // Pick constellation randomly from ConstellationList
        int randomIndex = Random.Range(0, constellationManager.Length); // Get random index
        // ConstellationScriptableObject constellationData = constellationManager[randomIndex];
        
        // Have starting instructions for person to get them into position
        SpawnStars(randomIndex);
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

        for (int i = 0; i < constellationManager[index].starLocations.Length; i++)
        {
            var _instantiatedStar = Instantiate(star, constellationManager[index].starLocations[i], Quaternion.identity);
            starList.Add(_instantiatedStar.GetComponent<IStar>());
        }
        
        
        for (int i = 0; i < constellationManager[index].startIndices.Length; i++)
        {
            int startIndex = constellationManager[index].startIndices[i];
            int endIndex = constellationManager[index].endIndices[i];

            IStar startStar = starList[startIndex];
            IStar endStar = starList[endIndex];
            startStar.StarGameObject.transform.position = constellationManager[index].starLocations[startIndex];
            endStar.StarGameObject.transform.position = constellationManager[index].starLocations[endIndex];
            
            (IStar, IStar) starTuple = (startStar, endStar);
            starTupleList.Add(starTuple);
        }

        constellation.Build(starTupleList);
    }
}
