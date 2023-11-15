using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfInstances;
    public float spawnDelay = 0.25f;
    public float positionFuzziness = 0.00001f; // Adjust this value to set the range of randomness
    private List<GameObject> spawnedParents; // List to keep track of the spawned parents

    void Start()
    {
        spawnedParents = new List<GameObject>();
        StartCoroutine(SpawnPrefabs());
    }

    void Update()
    {
        // Check and spawn prefabs if any are missing in each parent
        foreach (var parent in spawnedParents)
        {
            if (parent != null)
            {
                MaintainPrefabCount(parent);
            }
        }
    }

    IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject newParent = new GameObject("BottomBunParent");
            newParent.transform.position = transform.position;
            spawnedParents.Add(newParent);
            yield return StartCoroutine(SpawnPrefabsInParent(newParent));
        }
    }

    IEnumerator SpawnPrefabsInParent(GameObject parent)
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            SpawnPrefab(parent);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnPrefab(GameObject parent)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness));

        GameObject instance = Instantiate(prefabToSpawn, parent.transform.position + randomOffset, Quaternion.identity);
        if (prefabToSpawn.CompareTag("BottomBun"))
        {
            // Create a new child GameObject for the bottom bun
            GameObject bottomBunChild = new GameObject("BottomBunChild");
            bottomBunChild.transform.parent = parent.transform;
            bottomBunChild.transform.position = instance.transform.position;
            instance.transform.parent = bottomBunChild.transform;
        }
        else
        {
            instance.transform.parent = parent.transform;
        }
    }

    void MaintainPrefabCount(GameObject parent)
    {
        int totalCount = 0;
        foreach (Transform child in parent.transform)
        {
            totalCount += child.childCount;
        }

        int missingCount = numberOfInstances - totalCount;
        for (int i = 0; i < missingCount; i++)
        {
            SpawnPrefab(parent);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
