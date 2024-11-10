using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    
    
    private void Start()
    {
        _starState = StarState.Idle;
        _idleParticleSystem.Stop();
        _lookAtParticleSystem.Stop();
        _selectedParticleSystem.Stop();
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
                }

                break;
            case StarState.LookAt:
                if (!_lookAtParticleSystem.isPlaying)
                {
                    _idleParticleSystem.Stop();
                    _lookAtParticleSystem.Play();
                    _selectedParticleSystem.Stop();
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
        _starState = StarState.LookAt;
    }

    public void NotLookingAt()
    {
        
    }

    public GameObject StarGameObject { get; }
}
