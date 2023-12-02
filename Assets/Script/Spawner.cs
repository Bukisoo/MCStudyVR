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
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject instance = SpawnPrefab(this.gameObject);
            spawnedObjects.Add(instance);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    GameObject SpawnPrefab(GameObject parent)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness),
            Random.Range(-positionFuzziness, positionFuzziness));

        GameObject instance = Instantiate(prefabToSpawn, parent.transform.position + randomOffset, Quaternion.identity);
        instance.name = prefabToSpawn.name;
        instance.transform.SetParent(parent.transform);
        return instance;
    }

    void Update()
    {
        MaintainPrefabCount(this.gameObject);
    }

    void MaintainPrefabCount(GameObject parent)
    {
        int currentChildCount = parent.transform.childCount;
        int prefabsToSpawn = numberOfInstances - currentChildCount;

        for (int i = 0; i < prefabsToSpawn; i++)
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
