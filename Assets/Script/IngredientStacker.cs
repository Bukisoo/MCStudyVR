using UnityEngine;

public class IngredientStacker : MonoBehaviour
{
    private static Vector3 lastStackPosition;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "BottomBun" tag
        if (collision.gameObject.CompareTag("BottomBun"))
        {
            // Get the parent of the bottom bun
            Transform parentObject = collision.transform.parent;

            // Set the ingredient's parent to be the bottom bun's parent
            transform.SetParent(parentObject);

            // If this is the first ingredient, initialize the lastStackPosition
            if (lastStackPosition == Vector3.zero)
            {
                lastStackPosition = collision.transform.position;
            }

            // Adjust position - stack the ingredient slightly above the last stacked ingredient
            transform.position = lastStackPosition + new Vector3(0, 0.1f, 0); // adjust 0.1f if needed for correct offset

            // Update the lastStackPosition
            lastStackPosition = transform.position;

            // Fully constrain the Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
