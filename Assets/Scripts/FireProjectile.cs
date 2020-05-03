using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Rigidbody _projectileRb;
    private GameObject _player;
    private Vector3 _initialDirection;

    // Start is called before the first frame update
    void OnEnable()
    {
        _projectileRb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player");

        // Reset previous velocity since enabling and disabling
        _projectileRb.velocity = Vector3.zero;

        // Find where the player is facing the moment the projectile is instantiated
        // This is to stop the projectile from changing direction if the player changes direction again
        _initialDirection = _player.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        _projectileRb.AddForce(_initialDirection * _speed, ForceMode.Impulse);
    }

    // Destroy the projectile if it hits anything
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
