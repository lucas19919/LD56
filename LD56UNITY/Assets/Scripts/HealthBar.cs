using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform enemy;    
    public Camera mainCamera;    
    public Slider healthSlider; 
    private Vector3 offset = new Vector3(0, 0.5f, 0);  

    public HealthManager healthManager;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.forward);

        transform.position = enemy.position + offset;

        healthSlider.value = (healthManager.Health / 99);

    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthSlider.value = currentHealth / maxHealth;
    }
}

