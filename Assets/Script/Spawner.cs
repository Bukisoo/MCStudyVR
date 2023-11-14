using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfInstances;
    public float spawnDelay = 0.25f;
    public float positionFuzziness = 0.00001f; // Adjust this value to set the range of randomness
    private List<GameObject> spawnedInstances;

    void Start()
    {
        spawnedInstances = new List<GameObject>();
        StartCoroutine(SpawnPrefabs());
    }

    void Update()
    {
        // Check and spawn prefabs if any are missing
        MaintainPrefabCount();
    }

    IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            if (spawnedInstances.Count < numberOfInstances)
            {
                SpawnPrefab();
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    void SpawnPrefab()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness));

        GameObject instance = Instantiate(prefabToSpawn, transform.position + randomOffset, Quaternion.identity);
        instance.transform.parent = this.transform;
        spawnedInstances.Add(instance);
    }

    void MaintainPrefabCount()
    {
        spawnedInstances.RemoveAll(item => item == null);

        while (spawnedInstances.Count < numberOfInstances)
        {
            SpawnPrefab();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
