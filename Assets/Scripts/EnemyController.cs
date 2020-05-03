using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem _explosionParticle;

    [SerializeField] 
    private int _scorePerEnemy;

    [SerializeField]
    private AudioSource _hurtAudio;

    [SerializeField]
    private AudioSource _deathAudio;

    private GameManager _gameManager;
    private GameObject _player;
    private NavMeshAgent _navMeshAgent;
    private Animator _enemyAnim;
    private BoxCollider _boxCollider;
    private HealthManagement _enemyHealth;

    private bool _movingAnimationPlaying;
    private bool _enemyIsDead;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAnim = GetComponentInChildren<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
        _enemyHealth = GetComponent<HealthManagement>();

        _player = GameObject.Find("Player");
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IsGameActive)
        {
            // Enemy moves towards the player
            _navMeshAgent.SetDestination(_player.transform.position);

            // If the moving animation is not playing, then play it
            if (!_movingAnimationPlaying)
            {
                _enemyAnim.SetTrigger("Move");
                _movingAnimationPlaying = true;
                _navMeshAgent.isStopped = false;
            }
        }
    }

    // Play the death animation and wait for a second before destroying the enemy game object
    IEnumerator PlayDeathAnimation()
    {
        _enemyIsDead = true;
        _navMeshAgent.isStopped = true;
        _boxCollider.enabled = false;
        _deathAudio.Play();
        _enemyAnim.SetTrigger("Death");

        yield return new WaitForSeconds(1);

        SetPrefabBackToOriginalState();
    }

    // De-activate prefab and set everything back to its original state, to be used again for object pooling
    private void SetPrefabBackToOriginalState()
    {
        gameObject.SetActive(false);
        _enemyIsDead = false;
        _boxCollider.enabled = true;
        _movingAnimationPlaying = false;
        _enemyHealth.Health = _enemyHealth.MaxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If a projectile collides with the enemy, destroy the enemy
        if (collision.gameObject.CompareTag("Projectile") && !_enemyIsDead)
        {
            // Add to total score when an enemy is destroyed
            _gameManager.UpdateTotalScore(_scorePerEnemy);

            _enemyHealth.Health = 0;

            StartCoroutine(PlayDeathAnimation());
        }

        // If enemy has touched player, player loses 1 health and enemy stops moving
        if (collision.gameObject.CompareTag("Player") && !_enemyIsDead)
        {
            _enemyAnim.SetTrigger("Damage");
            _gameManager.UpdateHealth(-25);
            _navMeshAgent.isStopped = true;
            _movingAnimationPlaying = false;
            _hurtAudio.Play();
            _explosionParticle.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If enemy is no longer touching player, enemy carries on following player
        if (collision.gameObject.CompareTag("Player") && !_enemyIsDead)
        {
            _explosionParticle.Stop();
            _navMeshAgent.isStopped = false;
            _movingAnimationPlaying = false;
        }
    }
}
