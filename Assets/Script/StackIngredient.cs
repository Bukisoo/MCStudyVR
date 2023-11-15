using UnityEngine;

public class StackIngredient : MonoBehaviour
{
    public Transform attachmentPoint; // Set this to the point where the next ingredient should attach

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        Debug.Log($"Collision detected with object: {other.name}");

        // Check if both objects are ingredients and the other object is not already a child
        if (other.CompareTag("Ingredient") && gameObject.CompareTag("Ingredient") && other.transform.parent == null)
        {
            Debug.Log("Stacking ingredient.");

            // Calculate the position for the new ingredient
            Vector3 newPosition = attachmentPoint != null ? attachmentPoint.position : transform.position;
            if (transform.childCount > 0) // There are already other ingredients attached
            {
                // Adjust the newPosition to be slightly above the highest child
                float highestPoint = CalculateHighestPoint();
                newPosition.y = highestPoint + 0.1f; // Adjust this value to set the gap between ingredients
                Debug.Log($"New position set above the highest child. Y-Position: {newPosition.y}");
            }
            else
            {
                Debug.Log("Stacking the first ingredient.");
            }

            // Set this object to be the parent of the collided object
            other.transform.SetParent(transform);
            other.transform.position = newPosition;
            other.transform.rotation = Quaternion.identity; // Adjust if necessary

            // Optionally, disable physics on the child to prevent further collisions
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log("Physics disabled on the stacked ingredient.");
            }
        }
        else
        {
            Debug.Log("Collision with non-ingredient or already child object.");
        }
    }

    private float CalculateHighestPoint()
    {
        float highestPoint = 0f;
        foreach (Transform child in transform)
        {
            if (child.position.y > highestPoint)
            {
                highestPoint = child.position.y;
            }
        }
        return highestPoint;
    }
}
