using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private Coroutine _flashRoutine;
    private Color _originalColor;
    private float _duration = 0.03f;
    
    [Tooltip("Material and Color of flash")]
    [SerializeField] private Material flashMaterial;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        _originalColor = flashMaterial.color;
        
        flashMaterial = new Material(flashMaterial);
    }

    private IEnumerator FlashRoutine(Color color)
    {
        _spriteRenderer.material = flashMaterial;
        
        yield return new WaitForSeconds(_duration * 0.5f);
        
        flashMaterial.color = color;

        yield return new WaitForSeconds(_duration * 0.5f);

        _spriteRenderer.material = _originalMaterial;
        flashMaterial.color = _originalColor;

        _flashRoutine = null;
    }

    public void Flash(Color color)
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }

        _flashRoutine = StartCoroutine(FlashRoutine(color));
    }
}
