using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Star : MonoBehaviour, IStar
{
    public enum StarState
    {
        Idle = 0,
        LookAt = 1,
        Selected = 2
    };
    
    [SerializeField] private ParticleSystem _idleParticleSystem;
    [SerializeField] private ParticleSystem _lookAtParticleSystem;
    [SerializeField] private ParticleSystem _selectedParticleSystem;
    [SerializeField] private StarState _starState;
    [SerializeField] private float _timeToLookAtToConfirm = 3f;
    [SerializeField] Material _lookAtParticleSystemMaterial;
    [SerializeField] float _timeSinceLastLookedAt = 0f;
    [SerializeField] Color _defaultLookAtEmissionColor = new Color(0.964f, 0.992f, 0f);
    private float _lookAtMaterialEmissionColorItensifier;
    private ParticleSystem.MainModule _particleSystemLookAtMain;
    
    
    private void Start()
    {
        _starState = StarState.Idle;
        _idleParticleSystem.Stop();
        _lookAtParticleSystem.Stop();
        _selectedParticleSystem.Stop();
        _lookAtMaterialEmissionColorItensifier = 1f / _timeToLookAtToConfirm;
        _lookAtParticleSystemMaterial.SetColor("_EmissionColor", _defaultLookAtEmissionColor);
        _particleSystemLookAtMain = _lookAtParticleSystem.main;
    }

    private void Update()
    {
        switch (_starState)
        {
            case StarState.Idle:
                if (! _idleParticleSystem.isPlaying)
                {
                    _idleParticleSystem.Play();
                    _lookAtParticleSystem.Stop();
                    _selectedParticleSystem.Stop();

                    _timeSinceLastLookedAt = 0f;
                    _lookAtParticleSystem.startLifetime = 5f;
                    _lookAtParticleSystemMaterial.SetColor("_EmissionColor", _defaultLookAtEmissionColor);
                }
                break;
            case StarState.LookAt:
                if (!_lookAtParticleSystem.isPlaying)
                {
                    _idleParticleSystem.Stop();
                    _lookAtParticleSystem.Play();
                    _selectedParticleSystem.Stop();
                }

                if (_timeSinceLastLookedAt < _timeToLookAtToConfirm)
                {
                    _timeSinceLastLookedAt += Time.deltaTime;
                    if (_particleSystemLookAtMain.startLifetime.constant > 0f)
                    {
                        _particleSystemLookAtMain.startLifetime = 5f - _timeSinceLastLookedAt / _timeToLookAtToConfirm * 4.2f;
                        if (_lookAtParticleSystemMaterial.color.b + _timeSinceLastLookedAt * _lookAtMaterialEmissionColorItensifier < 1)
                        {
                            _lookAtParticleSystemMaterial.SetColor("_EmissionColor", new Color(_lookAtParticleSystemMaterial.color.r, _lookAtParticleSystemMaterial.color.g, _timeSinceLastLookedAt * _lookAtMaterialEmissionColorItensifier));
                            
                        }
                    }
                    else
                    {
                        _starState = StarState.Idle;
                    }
                }
                break;
            case StarState.Selected:
                if (!_selectedParticleSystem.isPlaying)
                {
                    _idleParticleSystem.Stop();
                    _lookAtParticleSystem.Stop();
                    _selectedParticleSystem.Play();
                }
                break;
        }
    }


    public void Confirmed()
    {
        _starState = StarState.Selected;
    }

    public void LookingAt()
    {
        if (_starState == StarState.Idle)
        {
            _starState = StarState.LookAt;
        }
    }

    public void NotLookingAt()
    {
        if (_starState == StarState.LookAt)
        {
            _starState = StarState.Idle;
        }
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
