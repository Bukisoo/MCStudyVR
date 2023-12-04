using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

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

    // Ensure the instance has a BoxCollider
    BoxCollider collider = instance.GetComponent<BoxCollider>();
    if (collider == null)
    {
        collider = instance.AddComponent<BoxCollider>();
        // Optionally set as trigger
        // collider.isTrigger = true;
    }

    // Ensure the instance has an XRGrabInteractable
    XRGrabInteractable grabInteractable = instance.GetComponent<XRGrabInteractable>();
    if (grabInteractable == null)
    {
        grabInteractable = instance.AddComponent<XRGrabInteractable>();
    }
    grabInteractable.colliders.Add(collider);

    // Ensure the instance has a Rigidbody
    Rigidbody rb = instance.GetComponent<Rigidbody>();
    if (rb == null)
    {
        rb = instance.AddComponent<Rigidbody>();
        // Set to kinematic if you don't want physics to affect the object
        rb.isKinematic = true;
    }

    //add the script "IngredientMerger" to the prefab
    IngredientMerger ingredientMerger = instance.GetComponent<IngredientMerger>();
    if (ingredientMerger == null)
    {
        ingredientMerger = instance.AddComponent<IngredientMerger>();
    }
    //Debug.Log("Spawned prefab with XRGrabInteractable and collider", instance);
    //print the value of the script "Ingredient" to the console
    Ingredient ingredient = instance.GetComponent<Ingredient>();

    //log the ingredient name to the console
    Debug.Log("Ingredient namec(spawner): " + ingredient.ingredientName);

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
