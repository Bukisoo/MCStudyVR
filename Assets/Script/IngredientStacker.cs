using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Make sure this namespace is included to access XRGrabInteractable

public class IngredientStacker : MonoBehaviour
{
    private static float nextIngredientHeight = 0.1f; // Initial offset, adjust as needed

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BottomBun"))
        {
            Transform parentObject = collision.transform.parent;
            transform.SetParent(parentObject);

            Vector3 newPosition = new Vector3(collision.transform.position.x, collision.transform.position.y + nextIngredientHeight, collision.transform.position.z);
            transform.position = newPosition;
            nextIngredientHeight += 0.1f; // adjust this for ingredient height

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            // Remove XRGrabInteractable from the ingredient
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                Destroy(grabInteractable);
            }

            // Add XRGrabInteractable to the parent if it doesn't already have one
            XRGrabInteractable parentGrabInteractable = parentObject.GetComponent<XRGrabInteractable>();
            if (parentGrabInteractable == null)
            {
                parentGrabInteractable = parentObject.gameObject.AddComponent<XRGrabInteractable>();
            }

            // Add a collider to the parent if it doesn't already have one
            Collider parentCollider = parentObject.GetComponent<Collider>();
            if (parentCollider == null)
            {
                // Adjust the type of collider based on your requirement
                parentObject.gameObject.AddComponent<BoxCollider>();
            }
        }
    }
}
