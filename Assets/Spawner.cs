using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfInstances;

    void Start()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject instance = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            instance.transform.parent = this.transform;
        }
    }
}
