using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed;

    private Rigidbody projectileRb;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileRb.AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
