using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfInstances;
    public float spawnDelay = 0.25f;
    public float positionFuzziness = 0.00001f;
    private List<GameObject> spawnedObjects;

    void Start()
    {
        spawnedObjects = new List<GameObject>();
        StartCoroutine(SpawnPrefabs());
    }

    IEnumerator SpawnPrefabs()
    {
        if (prefabToSpawn.CompareTag("BottomBun"))
        {
            // Spawn BottomBun prefabs each with a unique parent
            for (int i = 0; i < numberOfInstances; i++)
            {
                GameObject newParent = new GameObject("BottomBunParent");
                newParent.transform.position = transform.position;
                spawnedObjects.Add(newParent);

                SpawnPrefab(newParent);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        else
        {
            // Spawn other prefabs directly up to the specified count
            for (int i = 0; i < numberOfInstances; i++)
            {
                GameObject instance = SpawnPrefab(this.gameObject);
                spawnedObjects.Add(instance);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    GameObject SpawnPrefab(GameObject parent)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness));

        GameObject instance = Instantiate(prefabToSpawn, parent.transform.position + randomOffset, Quaternion.identity);
        instance.transform.SetParent(parent.transform);
        return instance;
    }

    void Update()
    {
        // Check and spawn prefabs if any are missing
        foreach (var obj in spawnedObjects)
        {
            if (obj != null && obj.CompareTag("BottomBunParent"))
            {
                MaintainPrefabCount(obj);
            }
        }
    }

    void MaintainPrefabCount(GameObject parent)
    {
        if (parent.transform.childCount < 1)
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
