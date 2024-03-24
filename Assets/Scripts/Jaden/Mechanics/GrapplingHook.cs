using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour 
{
    private PlayerMovement _thePlayer;
    
    private LineRenderer _line;

    [SerializeField] private bool isGamepad;

    [Header("Grapple Settings")]
    [SerializeField] private LayerMask grappleMask;
    [SerializeField] private float maxDistance;
    [SerializeField] private float grappleSpeed;
    [SerializeField] private float grappleShootSpeed;

    private bool _isGrappling = false;
    private bool _canGrapple;
    
    [HideInInspector] public bool retracting = false;

    private Vector2 _target;
    private Vector2 _aim;
    private Vector2 _aimDirection;
    private Vector2 _aimInput;

    private void Start()
    {
        _thePlayer = FindFirstObjectByType<PlayerMovement>();
        
        _line = GetComponent<LineRenderer>();

        _canGrapple = true;
    }

    private void Update()
    {
        HandleAim();
        
        if (retracting)
        {
            Vector2 grapplePos = Vector2.Lerp(transform.position, _target, grappleSpeed * Time.deltaTime);

            transform.position = grapplePos;

            _line.SetPosition(0, transform.position);

            if (Vector2.Distance(transform.position, _target) < 2.5f)
            {
                _thePlayer.CanMoveTrue();
                
                retracting = false;
                _isGrappling = false;
                _line.enabled = false;
            }
        }
    }

    public void StartGrapple()
    {
        if (PauseMenu.isPaused)
            return;
        
        if (!_isGrappling && _canGrapple)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _aimDirection, maxDistance, grappleMask);

            if (hit.collider != null)
            {
                _isGrappling = true;
                _target = hit.point;
                _line.enabled = true;
                _line.positionCount = 2;

                StartCoroutine(Grapple());
            }
        }
    }

    IEnumerator Grapple() 
    {
        float t = 0;
        float time = 5;

        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, transform.position);

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            var newPos = Vector2.Lerp(transform.position, _target, t / time);
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, newPos);
            yield return null;
        }
        
        _line.SetPosition(1, _target);
        
        _thePlayer.CanMoveFalse();
        
        retracting = true;
    }

    public void GetAim(Vector2 dir)
    {
        _aim = dir;
    }

    private void HandleAim()
    {
        if (isGamepad)
        {
            _aimInput = Gamepad.current.rightStick.ReadValue();
        
            _aimInput.Normalize();

            _aimDirection = new Vector2(_aimInput.x, _aimInput.y);
        }
        else
        {
            _aimDirection = Camera.main.ScreenToWorldPoint(_aim) - transform.position;
        }
    }
    
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Controller") ? true : false;
    }

    public void CanGrappleTrue()
    {
        _canGrapple = true;
    }

    public void CanGrappleFalse()
    {
        _canGrapple = false;
    }
}