using UnityEngine;

public class MoveToTower : MonoBehaviour
{
    public Transform tower;  // Assign this in the inspector with your tower object
    public float speed = 5.0f;  // Speed at which the force moves the creature towards the tower
    private Rigidbody rb;  // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this game object");
        }
    }

    void FixedUpdate()
    {
        if (tower != null && rb != null)
        {
            Vector3 direction = (tower.position - transform.position).normalized;  // Get the direction to the tower
            rb.AddForce(direction * speed);  // Apply a force towards the tower
        }
    }
}
