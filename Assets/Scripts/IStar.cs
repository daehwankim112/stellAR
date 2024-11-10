using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStar
{
    public void Confirmed();
    public void LookingAt();
    public void NotLookingAt();
    public Vector3 GetPosition();
    public void SetPosition(Vector3 position);
}
