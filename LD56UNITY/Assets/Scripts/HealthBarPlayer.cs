using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour
{
    public HealthManager healthManager;
    private Slider healthSlider;

    void Update()
    {
        healthSlider = this.gameObject.GetComponent<Slider>();
        healthSlider.value = (healthManager.Health) / 99;
    }
}
