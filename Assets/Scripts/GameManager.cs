
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class GameManager : MonoBehaviour
{

    public GameObject star;
    public GameObject parent;
    public ConstellationScriptableObject[] constellationManager;
    public int constellationNumber;
    public Constellation constellation;
    public List<(IStar, IStar)> starTupleList = new();
    
    [SerializeField] List<Star> starList = new();
    [SerializeField] GazeManager gazeManager;
    
    void Start()
    {
        // Pick constellation randomly from ConstellationList
        int randomIndex = Random.Range(0, constellationManager.Length); // Get random index
        // ConstellationScriptableObject constellationData = constellationManager[randomIndex];
        
        // Have starting instructions for person to get them into position
        SpawnStars();
        // constellation.Build();
        
    }

    void Update()
    {
        //if (constellation is complete) {
        //  constellationNumber++;
        //  SpawnStars();
        //}
    }



    private void SpawnStars()
    {
        // for (int i = 0; i < constellationManager[constellationNumber].starLocations.Length; i++)
        // {
        //     GameObject currentEntity = Instantiate(star);
        //     currentEntity.transform.position = constellationManager[constellationNumber].starLocations[i];
        //     currentEntity.transform.SetParent(parent.transform);
        //     currentEntity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        // }
        
        starList.Clear();
        
        Debug.Log("star location length: " + constellationManager[constellationNumber].starLocations.Length);

        for (int i = 0; i < constellationManager[constellationNumber].starLocations.Length; i++)
        {
            var _instantiatedStar = Instantiate(star, constellationManager[constellationNumber].starLocations[i], Quaternion.identity);
            starList.Add(_instantiatedStar.GetComponent<Star>());
        }
        
        
        for (int i = 0; i < constellationManager[constellationNumber].startIndices.Length; i++)
        {
            int startIndex = constellationManager[constellationNumber].startIndices[i];
            int endIndex = constellationManager[constellationNumber].endIndices[i];
            
            Star startStar = starList[startIndex];
            Star endStar = starList[endIndex];
            if (startStar != null && endStar != null)
            {
                startStar.Position = constellationManager[constellationNumber].starLocations[startIndex];
                endStar.Position = constellationManager[constellationNumber].starLocations[endIndex];
            }

            (IStar, IStar) starTuple = (startStar as IStar, endStar as IStar);
            
            starTupleList.Add(starTuple);
        }

        constellation = new();
        constellation.Build(starTupleList);
        constellation.OnComplete += OnConstellationComplete;
        gazeManager.GiveStarList(starList.ToArray(), constellation);
        
    }



    private void OnConstellationComplete(object sender, System.EventArgs e)
    {
        constellationNumber = (constellationNumber + 1) % constellationManager.Length;
        SpawnStars();
    }
}
