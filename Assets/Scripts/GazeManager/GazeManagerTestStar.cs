
using UnityEngine;



public class GazeManagerTestStar : MonoBehaviour, IStar
{
    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    

    public void LookingAt()
    {
        Debug.Log("Star is looked at");
        transform.Rotate(Vector3.forward, 180.0f * Time.deltaTime);
    }



    public void Confirmed()
    {
        Debug.Log("Star is selected");
        transform.Rotate(Vector3.up, 15.0f);
    }



    public void NotLookingAt()
    {
        
    }
}
