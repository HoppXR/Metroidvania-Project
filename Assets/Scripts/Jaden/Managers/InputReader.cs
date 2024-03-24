using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private static InputManager _input;
    
    private static PlayerInput _playerInput;

    public static InputReader Instance;
    
    public bool MenuOpenCloseInput { get; private set; }

    private InputAction _menuOpenCloseAction;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }

    private void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }

    public static void Init(PlayerMovement myPlayer)
    {
        _input = new InputManager();
        
        _input.Player.Enable();

        _input.Player.Movement.performed += ctx =>
        {
            myPlayer.SetMovementDirection(ctx.ReadValue<Vector2>());
        };
        _input.Player.Movement.canceled += ctx =>
        {
            myPlayer.SetMovementDirection(Vector2.zero);
        };

        _input.Player.Aim.performed += ctx =>
        {
            myPlayer.GrappleHook.GetAim(ctx.ReadValue<Vector2>());
        };

        _input.Player.Dash.performed += ctx =>
        {
            myPlayer.PlayerDash();
        };

        _input.Player.Grapple.performed += ctx =>
        {
            myPlayer.GrappleHook.StartGrapple();
        };

        _input.Player.Attack.performed += ctx =>
        {
            myPlayer.Combat.Attack();
        };
    }

    public static void SetPlayerControls()
    {
        _input.Player.Enable();
        _input.UI.Disable();
    }

    public static void SetUIControls()
    {
        _input.Player.Disable();
        _input.UI.Enable();
    }
}
