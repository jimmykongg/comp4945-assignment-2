using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private string playerID;
    
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveX, 0, 0);
    }

    public void Initialize(string id)
    {
        playerID = id;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("FallingObject"))
        {
            Debug.Log(playerID + " has been hit and eliminated!");
            Destroy(gameObject); // Remove player from game
        }
    }
}