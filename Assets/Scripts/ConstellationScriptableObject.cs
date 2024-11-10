
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ConstellationScriptableObject", order = 1)]
public class ConstellationScriptableObject : ScriptableObject, IStar
{
    public Dictionary<IStar, IStar[]> starData;
    public Vector2[] starAngles;
    public int[] startIndices;
    public int[] endIndices;

    void IStar.LookingAt() { }
    void IStar.NotLookingAt() { }
    void IStar.Confirmed() { }
    public Vector3 Position
    {
        get => Vector3.zero;
        set => _ = value;
    }
}
