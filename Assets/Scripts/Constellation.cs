using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Constellation : IConstellation
{ 
    private Dictionary<IStar, List<IStar>> _starAdjencyList;
    private Dictionary<IStar,int> _starMaxConnectionMap;
    private Dictionary<IStar, int> _starCurrConnectionMap;
    private int _completeStarCount = 0; // this should be used, instead of _selectedStarCount
    private FinalizedLineManager _lineManager;
    public List<GameObject> lineList= new();
    
    private IStar _prevSelectedStar;
    
    public event EventHandler<EventArgs> OnComplete;
    
    public Vector3? PrevStarPosition
    {
        get
        {
            return _prevSelectedStar != null ? _prevSelectedStar.Position : (Vector3?)null;
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
        _lineManager = new FinalizedLineManager();
        
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
            _starCurrConnectionMap[edge.Item1] = _starCurrConnectionMap.GetValueOrDefault(edge.Item1);
            _starCurrConnectionMap[edge.Item2] = _starCurrConnectionMap.GetValueOrDefault(edge.Item2);

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
             if (_prevSelectedStar != null)
             {
                 // _lineManager.DrawLine(_prevSelectedStar.Position, star.Position, isNextLineDisconnected);
                 GameObject line = new("Line");
                 LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                 // lineRenderer.material = 
                 lineRenderer.startWidth = 0.002f;
                 lineRenderer.endWidth = 0.002f;
                 Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
                 lineRenderer.material = whiteDiffuseMat;
                 lineRenderer.positionCount = 2;
                 lineRenderer.SetPosition(0, _prevSelectedStar.Position);
                 lineRenderer.SetPosition(1, star.Position);
                 lineList.Add(line);
             } 
             
             if (IsConstellationComplete())
             {
                 _prevSelectedStar = null;
                 ConstellationComplete();
             }
            
            UpdatePrevSelectedStar(star, isNextLineDisconnected);
            if (_prevSelectedStar != null)
            {
                Debug.Log("prevStar: " + _prevSelectedStar.Position.x);
            }
            else
            {
                Debug.Log("prevStar is NULL!");
            }
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
        Debug.Log("prev Star: ");
        UpdateStarConnection(_prevSelectedStar);
        Debug.Log("current Star: ");
        UpdateStarConnection(star);
    }

    private void UpdateStarConnection(IStar currStar)
    {
        // update currStar's currConnection
        _starCurrConnectionMap[currStar]++;
        Debug.Log("Star max count: " + _starMaxConnectionMap[currStar]);
        Debug.Log("Star curr count: " + _starCurrConnectionMap[currStar]);
        // if all connections made for currStar, update completeStarCount
        if (_starCurrConnectionMap[currStar] == _starMaxConnectionMap[currStar])
        {
            _completeStarCount++;
            Debug.Log("complete Star count: " + _completeStarCount);
        }
    }

    private bool isValidStarSelection(IStar star)
    {
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
        Debug.Log("Constellation Complete!!");
        OnComplete?.Invoke(this, EventArgs.Empty);
        return;
    }
}
