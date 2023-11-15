using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientMerger : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is also an ingredient
        IngredientMerger otherIngredient = collision.gameObject.GetComponent<IngredientMerger>();
        if (otherIngredient != null)
        {
            // Check if both ingredients are currently being held
            if (IsBeingHeld() && otherIngredient.IsBeingHeld())
            {
                // Child this ingredient to the other
                transform.SetParent(collision.transform);

                // Remove XRGrabInteractable from this ingredient
                if (grabInteractable != null)
                {
                    Destroy(grabInteractable);
                    //remove the IngredientMerger script from the object
                    Destroy(this);
                    //remove the box collider from the object
                    Destroy(GetComponent<BoxCollider>());
                }
            }
        }
    }

    private bool IsBeingHeld()
    {
        return grabInteractable.isSelected;
    }
}
