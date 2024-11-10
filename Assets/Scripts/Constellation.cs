using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : IConstellation
{
    private Dictionary<IStar, List<IStar>> _starAdjencyList;
    private Dictionary<IStar,Boolean> _starSelectMap;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookingAt(IStar star)
    {
        // Given star is being looked at and correct
        // Check if that star is already selected
        // If it is selected, pass
        // Else, mark star selected in our list and call star.Select
    }

    public void Selected(IStar star)
    {
        return;
    }

    public void Build((IStar, IStar)[] stars)
    {
        _starAdjencyList = new Dictionary<IStar, List<IStar>>();
        _starSelectMap = new Dictionary<IStar, Boolean>();
        
        foreach ((IStar, IStar) edge in stars)
        {
            _starAdjencyList[edge.Item1].Add(edge.Item2);
            _starAdjencyList[edge.Item2].Add(edge.Item1);
        }
    }
}
