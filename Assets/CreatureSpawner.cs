using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject creaturePrefab;  // Drag the BasicCreature prefab to this field in the Inspector
    public GameObject tower;  // Assign the tower GameObject in the Inspector
    public float spawnInterval = 5.0f; // Time interval between spawns

    private float planeSizeX;
    private float planeSizeZ;

    void Start()
    {
        // Assuming the plane's scale reflects its size in the world
        planeSizeX = this.transform.localScale.x * 9; // Multiplying by 10 assuming each scale unit is 10 world units
        planeSizeZ = this.transform.localScale.z * 9;
        StartCoroutine(SpawnCreature());
    }

    IEnumerator SpawnCreature()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Generate a random position on the edge of the plane
            Vector3 spawnPosition = RandomEdgePosition();
            GameObject creature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
            creature.transform.localScale = new Vector3(1, 1, 1); // Ensure the prefab scale is correct

            // Set the tower target for the MoveToTower script
            MoveToTower moveToTower = creature.GetComponent<MoveToTower>();
            if (moveToTower != null)
            {
                moveToTower.tower = tower.transform;  // Assigning the tower's Transform to the creature's script
            }
            else
            {
                Debug.LogWarning("MoveToTower script not found on the spawned creature.");
            }
        }
    }

    Vector3 RandomEdgePosition()
    {
        Vector3 basePosition = this.transform.position;
        float x, z;
        
        if (Random.value > 0.5)
        {
            // Spawn along the X-axis
            x = Random.value > 0.5 ? basePosition.x + planeSizeX / 2 : basePosition.x - planeSizeX / 2;
            z = basePosition.z + Random.Range(-planeSizeZ / 2, planeSizeZ / 2);
        }
        else
        {
            // Spawn along the Z-axis
            x = basePosition.x + Random.Range(-planeSizeX / 2, planeSizeX / 2);
            z = Random.value > 0.5 ? basePosition.z + planeSizeZ / 2 : basePosition.z - planeSizeZ / 2;
        }

        return new Vector3(x, basePosition.y + 1, z); // Assuming y is the height at which creatures should spawn
    }
}
