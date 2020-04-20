using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private bool isOnGround = true;
    private Rigidbody playerRb;
    private int numberOfLivesLeft = 3;

    public float speed;
    public float jumpForce;
    public float gravityModifier;
    public GameObject projectilePrefab;
    public TextMeshProUGUI livesText;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    // Moves the player based on arrow key input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(Vector3.forward * speed * verticalInput, ForceMode.Impulse);
        playerRb.AddForce(Vector3.right * speed * horizontalInput, ForceMode.Impulse);

        // Player must be on the ground before being allowed to jump again
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectilePrefab, transform.position + new Vector3(0, 1, 1), projectilePrefab.transform.rotation);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If player has collided with anything then allow it to jump again
        isOnGround = true;

        // If enemy has touched player, player loses 1 health
        if (collision.gameObject.CompareTag("Enemy"))
        {
            UpdateLivesLeft(-1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player collects a powerup, +1 to life and destroy the powerup
        if (other.gameObject.CompareTag("Powerup"))
        {
            UpdateLivesLeft(1);
            Destroy(other.gameObject);
        }
    }

    // Update lives text on UI
    private void UpdateLivesLeft(int lives)
    {
        numberOfLivesLeft += lives;
        livesText.text = $"Lives: {numberOfLivesLeft}/3";
    }
}
