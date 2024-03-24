using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Image healthBarColor;

    private float _lerpSpeed;

    void Start()
    {
        healthBarSlider.value = gameManager._playerHealth.Health;
    }
    
    private void Update()
    {
        _lerpSpeed = 3f * Time.deltaTime;
        
        HealthBarFiller();
        ColorChanger();
    }

    private void HealthBarFiller()
    {
        healthBarSlider.value = Mathf.Lerp(healthBarSlider.value, gameManager._playerHealth.Health, _lerpSpeed);
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, healthBarSlider.value / gameManager._playerHealth.MaxHealth);

        healthBarColor.color = healthColor;
    }
}
