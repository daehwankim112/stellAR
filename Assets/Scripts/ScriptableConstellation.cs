using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableConstellation : MonoBehaviour {
    public GameObject star;
    public GameObject parent;

    public ConstellationScriptableObject constellationManager;

    //void Start() {
      //  SpawnStars();
    //}

    public void SpawnStars() {
        for (int i = 0; i < constellationManager.starLocations.Length; i++) {
            GameObject currentEntity = Instantiate(star);
            currentEntity.transform.position = constellationManager.starLocations[i];
            currentEntity.transform.SetParent(parent.transform);
            currentEntity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
    }
}