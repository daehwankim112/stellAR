
using System;
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
    
    [SerializeField]
    List<Star> starList = new();

    [SerializeField]
    GazeManager gazeManager;

    [SerializeField]
    private GameObject _uiPrefab;
    private GameObject _uiGO;

    [SerializeField]
    private Transform _centerTransform;
    


    void Start()
    {
        _uiGO = Instantiate(_uiPrefab);
        _uiGO.GetComponent<MenuGazeSelector>().centerEyeTransform = _centerTransform;
        _uiGO.GetComponent<MenuGazeSelector>().StartEvent += OnStart;
    }



    private void OnStart(object obj, EventArgs e)
    {
        if (_uiGO != null)
        {
            _uiGO.GetComponent<MenuGazeSelector>().StartEvent -= OnStart;
            Destroy(_uiGO);
        }
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
        DestroyStars();
        
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
            
            starTupleList.Add((startStar, endStar));
        }

        constellation = new();
        constellation.Build(starTupleList);
        constellation.OnComplete += OnConstellationComplete;
        gazeManager.GiveStarList(starList.ToArray(), constellation);
        
    }



    private void DestroyStars()
    {
        for (int i = 0; i < starList.Count; i++)
        {
            Destroy(starList[i]);
        }

        starList.Clear();
    }



    private void OnConstellationComplete(object sender, System.EventArgs e)
    {
        constellationNumber = (constellationNumber + 1) % constellationManager.Length;
        constellation.OnComplete -= OnConstellationComplete;
        SpawnStars();
    }
}
