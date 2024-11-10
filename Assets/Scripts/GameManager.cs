
using System;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _starPrefab;
    public GameObject parent;

    public ConstellationScriptableObject[] constellationManager;

    [SerializeField]
    private int _constellationNumber;
    private Constellation _constellation;
    
    [SerializeField]
    private List<Star> _stars = new();

    [SerializeField]
    private GazeManager _gazeManager;

    [SerializeField]
    private GameObject _uiPrefab;
    private GameObject _uiGO;

    [SerializeField]
    private Transform _centerTransform;

    private StarGroup _starGroup;
    public float Scale;
    private float _curScale;
    public float Rotation;
    private float _curRotation;
    public float Radius;
    private float _curRadius;
    public Vector3 Offset;
    public Vector3 Center;
    private Vector3 _curPosition;
    private Vector3 _finalPosition;
    private Vector3 _curCenter;

    [SerializeField]
    private float _threshold;

    [SerializeField]
    private float _lerpSpeed;

    private bool _starsReady = false;
    


    void Start()
    {
        _uiGO = Instantiate(_uiPrefab);
        _uiGO.GetComponent<MenuGazeSelector>().centerEyeTransform = _centerTransform;
        _uiGO.GetComponent<MenuGazeSelector>().StartEvent += OnStart;
    }



    void Update()
    {
        if (_starGroup == null) return;
        if (!_starsReady)
        {
            MoveStars();
        }
        else if (_constellation == null)
        {
            CreateConstellation();
        }
    }


    private void MoveStars()
    {
        float lerpDelta = Time.deltaTime * _lerpSpeed;
        _curRadius = Mathf.Lerp(_curRadius, Radius, lerpDelta);
        _curCenter = Vector3.Slerp(_curCenter, Center, lerpDelta);
        _curScale = _starGroup.Spread;
        _curRotation = Mathf.LerpAngle(_curRotation, Rotation, lerpDelta);
        _curPosition = Vector3.Lerp(_curPosition, _finalPosition, lerpDelta);

        _starGroup.Radius = _curRadius;
        _starGroup.ReCenter(_curCenter);
        List<Vector3> scaledStars = _starGroup.ScaledRotatedTranslated(_curScale, _curRotation, _curPosition);


        for (int i = 0; i < _stars.Count; i++)
        {
            _stars[i].transform.position = scaledStars[i];
        }


        if (_curRadius > _threshold * Radius)
        {
            _starsReady = true;
        }
    }



    private void OnStart(object obj, EventArgs e)
    {
        if (_uiGO != null)
        {
            Destroy(_uiGO);
        }
        SpawnStars();
    }

    private void SpawnStars()
    {
        _starGroup = StarAligner.StarGroupFromRaDec(new List<Vector2>(constellationManager[_constellationNumber].starAngles));

        for (int i = 0; i < _starGroup.StarPositions.Count; i++)
        {
            var _instantiatedStar = Instantiate(_starPrefab, _starGroup.StarPositions[i], Quaternion.identity);
            _stars.Add(_instantiatedStar.GetComponent<Star>());
        }

        _curCenter = _starGroup.CenterOfMassSurface();
        _curPosition = _centerTransform.position;
        _curRadius = 0.0f;
        _curRotation = 180.0f;
        _curScale = _starGroup.Spread;
        _finalPosition = _centerTransform.position + Offset;
    }



    private void CreateConstellation()
    {
        List<Vector3> starPositions = _starGroup.ScaledRotatedTranslated(Scale, Rotation, _centerTransform.position + Offset);
        List<(IStar, IStar)> starTupleList = new();

        for (int i = 0; i < _starGroup.StarPositions.Count; i++)
        {
            int startIndex = constellationManager[_constellationNumber].startIndices[i];
            int endIndex = constellationManager[_constellationNumber].endIndices[i];
            
            Star startStar = _stars[startIndex];
            Star endStar = _stars[endIndex];
            if (startStar != null && endStar != null)
            {
                startStar.Position = starPositions[startIndex];
                endStar.Position = starPositions[endIndex];
            }
            
            starTupleList.Add((startStar, endStar));
        }

        _constellation = new();
        _constellation.Build(starTupleList);
        _constellation.OnComplete += OnConstellationComplete;
        _gazeManager.GiveStarList(_stars.ToArray(), _constellation);
    }



    private void Reset()
    {
        for (int i = 0; i < _stars.Count; i++)
        {
            Destroy(_stars[i].gameObject);
        }
        
        for (int i = 0; i < _constellation.lineList.Count; i++)
        {
            Destroy(_constellation.lineList[i]);
        }

        _stars.Clear();
        _starsReady = false;
        _constellation = null;
    }



    private void OnConstellationComplete(object sender, EventArgs e)
    {
        _constellationNumber = (_constellationNumber + 1) % constellationManager.Length;
        _constellation.OnComplete -= OnConstellationComplete;
        Reset();
        SpawnStars();
    }
}
