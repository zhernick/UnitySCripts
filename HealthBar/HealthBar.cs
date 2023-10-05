using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    [SerializeField] private Slider hpBarSlider, easeHpBarSlider;
    [SerializeField] float timeElapsed;
    [SerializeField] private float duration;
    [SerializeField] private bool clicked;

    private void Awake()
    {

    }

    private void Start()
    {
    }

    public void SetHealth(int maxHealth, int currentHealth)
    {
        hpBarSlider.maxValue = maxHealth;
        currentHealth = maxHealth;
        hpBarSlider.value = currentHealth;
        easeHpBarSlider.maxValue = maxHealth;
        easeHpBarSlider.value = currentHealth;
    }

    public void SmoothSliderHealthMovement(int currentHealth, ref bool value)
    {
        clicked = value;
        if (clicked)
        {
            timeElapsed += Time.deltaTime;
            var t = timeElapsed / duration;
            if (t <= duration * 2)
            {
                easeHpBarSlider.value = Mathf.MoveTowards(easeHpBarSlider.value, currentHealth, t);
            }
            else
            {
                value = false;
                clicked = value;
                timeElapsed = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {

        //UpdateSliderValues();
        //ResetSmoothTimer();
        clicked = true;
    }
    
    

    public void ResetSmoothTimer(bool value)
    {
        timeElapsed = 0;
        clicked = value;
    }

    public void UpdateSliderValues(int maxHealth, int currentHealth)
    {
        hpBarSlider.maxValue = maxHealth;
        hpBarSlider.value = currentHealth;
    }
}
