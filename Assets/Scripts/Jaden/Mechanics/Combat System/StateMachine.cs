
using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;

    private State mainStateType;
    
    public State CurrentState { get; private set; }
    private State _nextState;
    
    void Update()
    {
        if (_nextState != null)
        {
            SetState(_nextState);
            _nextState = null;
        }
        
        if (CurrentState != null)
            CurrentState.OnUpdate();
    }

    private void SetState(State newState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State newState)
    {
        if (newState != null)
        {
            _nextState = newState;
        }
    }

    private void LateUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnFixedUpdate();
    }
    
    public void SetNextStateToMain()
    {
        _nextState = mainStateType;
    }
    
    private void Awake()
    {
        SetNextStateToMain();

    }
    
    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}
