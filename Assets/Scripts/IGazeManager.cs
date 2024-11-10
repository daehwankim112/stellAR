using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGazeManager
{
    public void GiveStarList(IStar[] stars, IConstellation constellation);
}
