using UnityEngine;

public class CreatureCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collision detected");

        // Check if the collision is with the Tree
        if (collision.gameObject.tag == "Tower")
        {
            // Logic to handle the creature's "death"
            Destroy(gameObject);  // Destroys this creature GameObject
        }
    }
}
