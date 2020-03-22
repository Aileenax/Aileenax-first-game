using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravityModifier;
    public GameObject projectilePrefab;

    private bool isOnGround = true;
    private Rigidbody playerRb;

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

        playerRb.AddForce(Vector3.forward * speed * verticalInput);
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // Player must be on the ground before being allowed to jump again
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);

            Instantiate(projectilePrefab, position, projectilePrefab.transform.rotation);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If player has touched the ground set isOnGround back to true
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
