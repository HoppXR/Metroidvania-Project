using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour 
{
    private LineRenderer _line;
    
    private PlayerMovement _thePlayer;

    [Header("Grapple Settings")]
    [SerializeField] private LayerMask grappleMask;
    [SerializeField] private float maxDistance;
    [SerializeField] private float grappleSpeed;
    [SerializeField] private float grappleShootSpeed;

    private bool _isGrappling = false;
    private bool _canGrapple;
    
    [HideInInspector] public bool retracting = false;

    private Vector2 _target;

    private void Start() 
    {
        _thePlayer = FindObjectOfType<PlayerMovement>();
        
        _line = GetComponent<LineRenderer>();

        _canGrapple = true;
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0) && !_isGrappling && _canGrapple) 
        {
            StartGrapple();
        }
        
        if (retracting)
        {
            Vector2 grapplePos = Vector2.Lerp(transform.position, _target, grappleSpeed * Time.deltaTime);

            transform.position = grapplePos;

            _line.SetPosition(0, transform.position);

            if (Vector2.Distance(transform.position, _target) < 1.2f || Input.GetKeyDown(KeyCode.Space))
            {
                _thePlayer.CanDashTrue();
                _thePlayer.CanMoveTrue();
                
                retracting = false;
                _isGrappling = false;
                _line.enabled = false;
            }
        }
    }

    private void StartGrapple() 
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grappleMask);

        if (hit.collider != null)
        {
            _thePlayer.CanDashFalse();
            _thePlayer.CanMoveFalse();
            
            _isGrappling = true;
            _target = hit.point;
            _line.enabled = true;
            _line.positionCount = 2;

            StartCoroutine(Grapple());
        }
    }

    IEnumerator Grapple() 
    {
        float t = 0;
        float time = 5;

        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, transform.position); 

        Vector2 newPos;

        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, _target, t / time);
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, newPos);
            yield return null;
        }
        
        _line.SetPosition(1, _target);
        retracting = true;
    }
}