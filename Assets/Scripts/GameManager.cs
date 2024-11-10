using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject star;
    public GameObject parent;
    public ConstellationScriptableObject[] constellationManager = new ConstellationScriptableObject[6];
    public int constellationNumber = 1;

    void Start() {
        // Have starting instructions for person to get them into position
        SpawnStars();
    }

    void Update() {
        //if (constellation is complete) {
        //  constellationNumber++;
        //  SpawnStars();
        //}
    }

    void SpawnStars() {
        for (int i = 0; i < constellationManager[constellationNumber].starLocations.Length; i++)
        {
            GameObject currentEntity = Instantiate(star);
            currentEntity.transform.position = constellationManager[constellationNumber].starLocations[i];
            currentEntity.transform.SetParent(parent.transform);
            currentEntity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }
}
