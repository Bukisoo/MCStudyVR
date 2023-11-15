using UnityEngine;

public class StackIngredient : MonoBehaviour
{
    private BoxCollider bottomBunCollider; // Collider for the bottom bun

    void Start()
    {
        // Ensure there's a BoxCollider on the bottom bun and store it for later use
        bottomBunCollider = gameObject.GetComponent<BoxCollider>();
        if (bottomBunCollider == null)
        {
            bottomBunCollider = gameObject.AddComponent<BoxCollider>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        Debug.Log($"Collision detected with object: {other.name} and tag: {other.tag}");

        if (other.CompareTag("Ingredient") && gameObject.CompareTag("BottomBun"))
        {
            Debug.Log("Attaching ingredient model to the parent of the bottom bun.");

            Transform parentTransform = transform.parent; // Get the parent of the bottom bun

            // Attach the ingredient to the parent of the bottom bun
            other.transform.SetParent(parentTransform);

            // Calculate and set the position for the new ingredient
            Vector3 newPosition = CalculateNextItemLocalPosition();
            newPosition += parentTransform.position; // Adjust for parent's position
            other.transform.position = newPosition;
            other.transform.rotation = Quaternion.identity; // Adjust if necessary

            // Optionally, disable physics on the ingredient to prevent further collisions
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log("Physics disabled on the stacked ingredient.");
            }

            // Adjust the collider of the bottom bun to encompass the entire stack
            AdjustBottomBunCollider();
        }
        else
        {
            Debug.Log("Collision with non-ingredient or non-bottom bun object.");
        }
    }
    private void AttachIngredientModel(GameObject ingredient)
    {
        // Assuming the ingredient has a MeshRenderer component
        MeshRenderer ingredientRenderer = ingredient.GetComponent<MeshRenderer>();
        if (ingredientRenderer != null)
        {
            // Create a new GameObject to hold the ingredient's model
            GameObject ingredientModel = new GameObject(ingredient.name + " Model");
            ingredientModel.transform.SetParent(transform);

            // Copy the MeshRenderer and MeshFilter from the ingredient to the new GameObject
            MeshRenderer modelRenderer = ingredientModel.AddComponent<MeshRenderer>();
            MeshFilter modelFilter = ingredientModel.AddComponent<MeshFilter>();

            modelRenderer.materials = ingredientRenderer.materials;
            modelFilter.mesh = ingredient.GetComponent<MeshFilter>().mesh;

            // Position the model at the top of the stack
            ingredientModel.transform.localPosition = CalculateNextItemLocalPosition();
            ingredientModel.transform.localRotation = Quaternion.identity;
        }
    }

    private Vector3 CalculateNextItemLocalPosition()
    {
        // Calculate the local position for the new ingredient model
        float yOffset = CalculateNextItemYOffset();
        return new Vector3(0f, yOffset, 0f);
    }
    private void AdjustBottomBunCollider()
    {
        float totalStackHeight = CalculateTotalStackHeight();
        // Adjust the collider size to encompass the entire stack
        bottomBunCollider.size = new Vector3(bottomBunCollider.size.x, totalStackHeight, bottomBunCollider.size.z);
        // Adjust the collider center so it remains centered on the stack
        bottomBunCollider.center = new Vector3(0, totalStackHeight / 2, 0);
    }

  private float CalculateNextItemYOffset()
    {
        if (transform.childCount == 0) // No ingredients yet
        {
            return bottomBunCollider.size.y * 0.5f; // Place the first ingredient on top of the bun
        }
        else
        {
            // Find the highest point of the current stack
            float highestPoint = CalculateHighestPoint();
            // The new item's y position is the highest point plus a 0.1 unit offset
            return highestPoint + 0.1f - transform.position.y;
        }
    }

    private float CalculateHighestPoint()
    {
        float highestPoint = bottomBunCollider.size.y * 0.5f; // Start with the height of the bun
        foreach (Transform child in transform)
        {
            BoxCollider childCollider = child.GetComponent<BoxCollider>();
            if (childCollider != null)
            {
                float childTopY = child.position.y + childCollider.size.y * 0.1f;
                if (childTopY > highestPoint)
                {
                    highestPoint = childTopY;
                }
            }
        }
        return highestPoint;
    }

    private float CalculateTotalStackHeight()
    {
        return CalculateHighestPoint() + 0.1f; // Add a small offset for the top of the stack
    }
}