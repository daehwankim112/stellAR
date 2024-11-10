using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : IConstellation
{ 
    private Dictionary<IStar, List<IStar>> _starAdjencyList;
    private Dictionary<IStar,int> _starMaxConnectionMap;
    private Dictionary<IStar, int> _starCurrConnectionMap;
    private int _completeStarCount = 0; // this should be used, instead of _selectedStarCount
    private LineManager _lineManager;
    
    private IStar _prevSelectedStar;
    
    public Vector3? PrevStarPosition
    {
        get
        {
            return _prevSelectedStar != null ? _prevSelectedStar.GetPosition() : (Vector3?)null;
        }
        private set { } // Keep the setter private if needed
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Build(List<(IStar, IStar)> stars)
    {
        _starAdjencyList = new Dictionary<IStar, List<IStar>>();
        _starMaxConnectionMap = new Dictionary<IStar, int>();
        _starCurrConnectionMap = new Dictionary<IStar, int>();
        _lineManager = new LineManager();
        
        foreach ((IStar, IStar) edge in stars)
        {
            if (_starAdjencyList.ContainsKey(edge.Item1) == false)
            {
                _starAdjencyList.Add(edge.Item1, new List<IStar>());
            }
            _starAdjencyList[edge.Item1].Add(edge.Item2);

            if (_starAdjencyList.ContainsKey(edge.Item2) == false)
            {
                _starAdjencyList.Add(edge.Item2, new List<IStar>());
            }
            _starAdjencyList[edge.Item2].Add(edge.Item1);
            
            _starMaxConnectionMap[edge.Item1] = _starMaxConnectionMap.GetValueOrDefault(edge.Item1) + 1;
            _starMaxConnectionMap[edge.Item2] = _starMaxConnectionMap.GetValueOrDefault(edge.Item2) + 1;
        }
        
        foreach (IStar star in _starAdjencyList.Keys)
        {
            foreach (var neighbor in _starAdjencyList[star])
            {
                Debug.Log("Check: " + star.GetPosition().x + "Neighbor: " + neighbor.GetPosition().x);
            }
        }
    }

    public void LookingAt(IStar star)
    {
        star.LookingAt();
    }

    public void Selected(IStar star)
    {
       
        // run the validation check 
        if (isValidStarSelection(star))
        {
            // update given star and prevStar's currConnection
            UpdateStarConnectionCount(star);
            
            star.Confirmed();

            bool isNextLineDisconnected = _starCurrConnectionMap[star] == _starMaxConnectionMap[star];
            
            // draw line
            _lineManager.DrawLine(_prevSelectedStar.GetPosition(), star.GetPosition(), isNextLineDisconnected);
            
            UpdatePrevSelectedStar(star, isNextLineDisconnected);
        }
    }

    private void UpdatePrevSelectedStar(IStar star, bool isNextLineDisconnected)
    {
        // check if currStar is deadend and need to find new star to draw line
        if (isNextLineDisconnected && !IsConstellationComplete())
        {
            _prevSelectedStar = null;
        }
        else
        {
            _prevSelectedStar = star;
        }
    }

    private void UpdateStarConnectionCount(IStar star)
    {
        if (_prevSelectedStar == null)
        {
            return;
        }
        // update given star and prevStar's currConnection
        UpdateStarConnection(_prevSelectedStar);
        UpdateStarConnection(star);
    }

    private void UpdateStarConnection(IStar currStar)
    {
        // update currStar's currConnection
        _starCurrConnectionMap[currStar]++;
        // if all connections made for currStar, update completeStarCount
        if (_starCurrConnectionMap[currStar] == _starMaxConnectionMap[currStar])
        {
            _completeStarCount++;
            
            // If all stars are selected, trigger ConstellationComplete
            if (IsConstellationComplete())
            {
                ConstellationComplete();
            }
        }
    }

    private bool isValidStarSelection(IStar star)
    {
        // if selected star is not selected yet and is neighbor of prevStar -> valid selection
        // if (no prevStar or star is neighbor of prevStar) and 
        if ((_prevSelectedStar == null || _starAdjencyList[_prevSelectedStar].Contains(star)) 
            && _starCurrConnectionMap[star] < _starMaxConnectionMap[star])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsConstellationComplete()
    {
        if (_completeStarCount == _starMaxConnectionMap.Count)
        {
            return true;
        }

        return false;
    }

    private void ConstellationComplete()
    {
        return;
    }
}
