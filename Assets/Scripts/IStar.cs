
using UnityEngine;



public interface IStar
{
    public Vector3 Position { get; set; }

    public void Confirmed();
    public void LookingAt();
    public void NotLookingAt();
}
