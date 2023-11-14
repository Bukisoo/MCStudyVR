using UnityEngine;

public class StackIngredient : MonoBehaviour
{
    public Transform attachmentPoint; // Set this to the point where the next ingredient should attach

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Check if the other object is an ingredient and not already a child of another object
        if (other.CompareTag("Ingredient") && other.transform.parent == null)
        {
            // Set this object to be the parent of the collided object
            other.transform.SetParent(transform);

            // Position the ingredient at the attachment point
            if (attachmentPoint != null)
            {
                other.transform.position = attachmentPoint.position;
                other.transform.rotation = attachmentPoint.rotation;
            }
            else
            {
                // Default behavior if no attachment point is specified
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;
            }

            // Optionally, disable physics on the child to prevent further collisions
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
}
