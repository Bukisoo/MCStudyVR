using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfInstances;
    public float spawnDelay = 0.25f;
    public float positionFuzziness = 0.00001f; // Adjust this value to set the range of randomness
    private List<GameObject> spawnedObjects; // List to keep track of the spawned objects or parents

    void Start()
    {
        spawnedObjects = new List<GameObject>();
        StartCoroutine(SpawnPrefabs());
    }

    void Update()
    {
        // Check and spawn prefabs if any are missing
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                if (prefabToSpawn.CompareTag("BottomBun"))
                {
                    MaintainPrefabCount(obj);
                }
                else if (!prefabToSpawn.CompareTag("BottomBun"))
                {
                    MaintainPrefabCount(this.gameObject);
                }
            }
        }
    }

    IEnumerator SpawnPrefabs()
    {
        if (prefabToSpawn.CompareTag("BottomBun"))
        {
            // Spawn BottomBun prefabs with a parent
            for (int i = 0; i < numberOfInstances; i++)
            {
                GameObject newParent = new GameObject("BottomBunParent");
                newParent.tag = "BottomBunParent"; // Tag the parent for identification
                newParent.transform.position = transform.position;
                spawnedObjects.Add(newParent);
                yield return StartCoroutine(SpawnPrefab(newParent));
            }
        }
        else
        {
            // Spawn other prefabs directly
            for (int i = 0; i < numberOfInstances; i++)
            {
                yield return StartCoroutine(SpawnPrefab(this.gameObject));
            }
        }
    }

    IEnumerator SpawnPrefab(GameObject parent)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness));

        GameObject instance = Instantiate(prefabToSpawn, parent.transform.position + randomOffset, Quaternion.identity);
        instance.transform.parent = parent.transform;
        yield return new WaitForSeconds(spawnDelay);
    }

    void MaintainPrefabCount(GameObject parent)
    {
        int childCount = parent.transform.childCount;
        for (int i = childCount; i < numberOfInstances; i++)
        {
            StartCoroutine(SpawnPrefab(parent));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
