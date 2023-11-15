using UnityEngine;

public class StackIngredient : MonoBehaviour
{
    public Transform attachmentPoint; // Set this to the point where the next ingredient should attach

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Check if this object is the bottom bun and the other object is an ingredient
        if (other.CompareTag("Ingredient") && gameObject.CompareTag("BottomBun"))
        {
            // Set this object to be the parent of the collided object
            other.transform.SetParent(transform);

            // Calculate the position for the new ingredient
            Vector3 newPosition = attachmentPoint != null ? attachmentPoint.position : transform.position;
            if (transform.childCount > 1) // There are already other ingredients attached
            {
                // Adjust the newPosition to be slightly above the highest child
                float highestPoint = 0f;
                foreach (Transform child in transform)
                {
                    if (child.position.y > highestPoint)
                    {
                        highestPoint = child.position.y;
                    }
                }
                newPosition.y = highestPoint + 0.1f; // Adjust this value to set the gap between ingredients
            }

            other.transform.position = newPosition;
            other.transform.rotation = Quaternion.identity; // Adjust if necessary

            // Optionally, disable physics on the child to prevent further collisions
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
}
