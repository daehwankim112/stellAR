using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConstellationScriptableObject", order = 1)]
public class ConstellationScriptableObject : ScriptableObject, IStar
{
    
    public Dictionary<IStar, IStar[]> starData;
    public Vector3[] starLocations;
    public int[] startIndices;
    public int[] endIndices;

    void IStar.LookingAt() { }
    void IStar.NotLookingAt() { }
    void IStar.Confirmed() { }

    Vector3 IStar.GetPosition()
    {
        return Vector3.zero;
    }
    void IStar.SetPosition(Vector3 position) { }
}
