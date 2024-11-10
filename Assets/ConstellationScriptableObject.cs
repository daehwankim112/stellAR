using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConstellationScriptableObject", order = 1)]
public class ConstellationScriptableObject : ScriptableObject, IStar
{
    public Dictionary<IStar, IStar[]> starData;
    public Vector3[] starLocations;

    void IStar.LookingAt() { }
    void IStar.Select() { }
    void IStar.NotLookingAt() { }
    void IStar.Confirmed() { }
}
