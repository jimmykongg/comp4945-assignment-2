using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5f;
    
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(0, -fallSpeed, 0);
        }
    }

    void Update()
    {
        // Destroy the object if it falls below a certain point
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + " hit " + other.gameObject.name + "! Removing both.");
            
            Destroy(other.gameObject); //Remove the player cube
            Destroy(gameObject); // Remove this sphere
        }
    }
}