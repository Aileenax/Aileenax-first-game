using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{
    // Properties with auto-implemented get and set
    public float Health { get; set; }
    // Properties with only get implemented, meaning its value cannot be assigned outside of this class
    public float MaxHealth { get; private set; } = 100;

    [SerializeField]
    private Slider _healthBar;

    private Camera _mainCamera;
    private Vector3 _mainCameraForward;

    void Start()
    {
        _mainCamera = Camera.main;
        _mainCameraForward = _mainCamera.transform.forward;

        // Newly spawned enemy always starts at max health
        Health = MaxHealth;
        _healthBar.value = CalculateHealth();
    }

    void Update()
    {
        _healthBar.value = CalculateHealth();
    }

    private void LateUpdate()
    {
        _healthBar.transform.forward = _mainCameraForward;
    }

    private float CalculateHealth()
    {
        // As we are using a slider, we need to work in percentages (max value of slider = 1)
        return Health / MaxHealth;
    }
}
