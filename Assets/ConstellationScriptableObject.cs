using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConstellationScriptableObject", order = 1)]
public class ConstellationScriptableObject : ScriptableObject, IStar
{
    public Dictionary<IStar, IStar[]> starData;
    public Vector3[] starLocations;

    void IStar.LookAt() { }
    void IStar.Select() { }
}
