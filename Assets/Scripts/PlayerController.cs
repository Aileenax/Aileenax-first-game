using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float _speed;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _gravityModifier;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private GameObject _projectilePrefab;

    [SerializeField]
    private ParticleSystem _jumpParticle;

    private Rigidbody _playerRb;
    private GameManager _gameManager;
    private Animator _playerAnim;
    private AudioSource _shootingAudio;
    private bool _isOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<Animator>();
        _shootingAudio = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        Physics.gravity *= _gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IsGameActive && !_gameManager.IsGamePaused)
        {
            MovePlayer();
        }
        else if (!_gameManager.IsGameActive && _gameManager.Health == 0)
        {
            // Game is over, play the death animation
            _playerAnim.SetBool("Death_b", true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_gameManager.IsGamePaused && _gameManager.IsGameActive)
        {
            _gameManager.PauseGame();
        }
    }

    // Moves the player based on arrow key input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput).normalized * Time.deltaTime * _speed;

        _playerRb.AddForce(move, ForceMode.Impulse);

        SetAnimationAndRotation(horizontalInput, verticalInput, move);

        // Player must be on the ground before being allowed to jump again
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            // Play the jumping animation
            _playerAnim.SetBool("Jump_b", true);
            _jumpParticle.Play();
            _playerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isOnGround = false;
        }

        // If pressing the fire button, a new bullet is pooled in front of where the player is facing
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = transform.position + transform.forward + Vector3.up;

            GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject("Projectile");

            if (pooledProjectile != null)
            {
                pooledProjectile.transform.position = spawnPos;
                pooledProjectile.transform.rotation = _projectilePrefab.transform.rotation;
                pooledProjectile.SetActive(true);
            }

            _shootingAudio.Play();
        }
    }

    private void SetAnimationAndRotation(float horizontalInput, float verticalInput, Vector3 move)
    {
        // If moving, set the walking animation and rotate the player to face the correct direction
        if (verticalInput != 0 || horizontalInput != 0)
        {
            _playerAnim.SetFloat("Speed_f", 0.3f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), _rotationSpeed);
        }

        // Not moving at all, set the idle animation
        if ((Mathf.Approximately(verticalInput, 0) && Mathf.Approximately(horizontalInput, 0)))
        {
            _playerAnim.SetFloat("Speed_f", 0.2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player has collided with anything then allow it to jump again
        // Stop playing the jumping animation
        _isOnGround = true;
        _playerAnim.SetBool("Jump_b", false);
        _jumpParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player collects a powerup, +1 to life and destroy the powerup
        if (other.gameObject.CompareTag("Powerup"))
        {
            _gameManager.UpdateHealth(25);
            other.gameObject.SetActive(false);
        }
    }
}
