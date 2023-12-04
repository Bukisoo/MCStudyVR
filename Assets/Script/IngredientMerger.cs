using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class IngredientMerger : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor rightHandInteractor;
    private XRBaseInteractor leftHandInteractor;
    
    private float currentPileHeight = 0f;
    private float yOffset = 0.1f;
    private float initialOffset = 0.1f; // Adjust this value to set the initial offset

    private BoxCollider triggerCollider;

    private void Awake()
    {
        currentPileHeight = initialOffset;
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component missing", this);
            return;
        }

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        // Add a secondary trigger collider
        triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
        UpdateTriggerColliderSize();
    }

// Update method using the new Input System
private void OnGrabbed(SelectEnterEventArgs arg)
    {
        //Debug.Log("Grabbed by interactor with tag: " + arg.interactor.tag, this);

        if (arg.interactor.CompareTag("RightHand"))
        {
            rightHandInteractor = arg.interactor;
            //Debug.Log(gameObject.name + " was grabbed by right hand", this);
        }
        else if (arg.interactor.CompareTag("LeftHand"))
        {
            leftHandInteractor = arg.interactor;
            //Debug.Log(gameObject.name + " was grabbed by left hand", this);

            // Set the collider's isTrigger property to true when grabbed by left hand
            SetColliderTriggerStatus(true);

            //log the value of the script "Ingredient" to the console of the ingredient that is grabbed
            Ingredient ingredient = GetComponent<Ingredient>();
            //Debug.Log("Ingredient name: (merger)" + ingredient.ingredientName);

        }
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        if (arg.interactor == rightHandInteractor)
        {
            rightHandInteractor = null;
            //Debug.Log(gameObject.name + " was released by right hand", this);
        }
        else if (arg.interactor == leftHandInteractor)
        {
            leftHandInteractor = null;
            //Debug.Log(gameObject.name + " was released by left hand", this);

            // Revert the collider's isTrigger property to false when released by left hand
            SetColliderTriggerStatus(false);
        }
    }

    private void SetColliderTriggerStatus(bool isTrigger)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = isTrigger;
        }
    }


private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Ingredient") && grabInteractable.isSelected)
    {
        IngredientMerger otherIngredient = other.GetComponent<IngredientMerger>();
        if (otherIngredient != null && otherIngredient.grabInteractable.isSelected)
        {
            // If this object is grabbed by the left hand and the other object is grabbed by the right hand
            if (leftHandInteractor != null && otherIngredient.rightHandInteractor != null)
            {
                MakeChildOf(otherIngredient, this);
            }
            // If this object is grabbed by the right hand and the other object is grabbed by the left hand
            else if (rightHandInteractor != null && otherIngredient.leftHandInteractor != null)
            {
                MakeChildOf(this, otherIngredient);
            }
        }
    }

}


  private void MakeChildOf(IngredientMerger child, IngredientMerger parent)
{
    string prefabPath = "Prefabs/" + child.gameObject.name;
    GameObject prefab = Resources.Load<GameObject>(prefabPath);
    if (prefab == null)
    {
        Debug.LogError("No prefab found for path: " + prefabPath);
        return;
    }

    GameObject newChild = Instantiate(prefab, parent.transform);
    newChild.name = prefab.name;

    Debug.Log("Instantiated new child: " + newChild.name);

    Ingredient newChildIngredient = newChild.GetComponent<Ingredient>();
    if (newChildIngredient == null)
    {
        Debug.LogError("Prefab Instantiation Issue: The instantiated object does not have the Ingredient component. Prefab name: " + newChild.name);
    }
    else
    {
        Debug.Log("Instantiated new ingredient: " + newChildIngredient.ingredientName);
    }

        Vector3 originalScale = child.transform.localScale;
        Vector3 inverseParentScale = new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z);
        newChild.transform.localScale = Vector3.Scale(originalScale, inverseParentScale);
        newChild.transform.localPosition = new Vector3(0, currentPileHeight, 0);
        newChild.transform.localRotation = Quaternion.identity;

        currentPileHeight += yOffset;

        Rigidbody rigidbody = newChild.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }

        XRGrabInteractable grabInteractable = newChild.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        BoxCollider boxCollider = newChild.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }

        Destroy(child.gameObject);

        // After merging, log the composition of the burger
        LogBurgerComposition(parent.gameObject);

        UpdateTriggerColliderSize();

    }

    private void UpdateTriggerColliderSize()
    {
        if (triggerCollider != null)
        {
            // Adjust the size and position of the trigger collider as needed
            // Here, I'm just using an example. You should set this according to your requirements.
            triggerCollider.size = new Vector3(0.1f, currentPileHeight, 0.1f);
            triggerCollider.center = new Vector3(0, currentPileHeight / 2, 0);
        }
    }

 private void LogBurgerComposition(GameObject burger)
    {
        List<string> ingredients = new List<string>();
        CollectIngredients(burger.transform, ref ingredients);

        string composition = "Burger Composition after merge: " + string.Join(", ", ingredients);
        Debug.Log(composition);

        // Count unique ingredients and their quantities
        var ingredientCounts = new Dictionary<string, int>();
        foreach (var ing in ingredients)
        {
            if (ingredientCounts.ContainsKey(ing))
            {
                ingredientCounts[ing]++;
            }
            else
            {
                ingredientCounts[ing] = 1;
            }
        }

        foreach (var pair in ingredientCounts)
        {
            Debug.Log("Ingredient: " + pair.Key + ", Quantity: " + pair.Value);
        }

        Debug.Log("Total number of ingredients (including parent and children): " + ingredients.Count);
    }

    private void CollectIngredients(Transform transform, ref List<string> ingredients)
    {
        Ingredient ingredient = transform.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            ingredients.Add(ingredient.ingredientName);
            Debug.Log("Collected ingredient: " + ingredient.ingredientName);
        }
        else
        {
            Debug.Log("No Ingredient component found on: " + transform.gameObject.name);
        }

        // Recursively collect ingredients from all children
        foreach (Transform child in transform)
        {
            CollectIngredients(child, ref ingredients);
        }
    }


}
