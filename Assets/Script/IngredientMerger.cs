using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientMerger : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor rightHandInteractor;
    private XRBaseInteractor leftHandInteractor;

    private void Awake()
    {
        Debug.Log("IngredientMerger Awake called", this);
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component missing", this);
            return;
        }

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        if (arg.interactor.CompareTag("RightHand"))
        {
            rightHandInteractor = arg.interactor;
            Debug.Log(gameObject.name + " was grabbed by right hand", this);
        }
        else if (arg.interactor.CompareTag("LeftHand"))
        {
            leftHandInteractor = arg.interactor;
            Debug.Log(gameObject.name + " was grabbed by left hand", this);
        }
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        if (arg.interactor == rightHandInteractor)
        {
            rightHandInteractor = null;
            Debug.Log(gameObject.name + " was released by right hand", this);
        }
        else if (arg.interactor == leftHandInteractor)
        {
            leftHandInteractor = null;
            Debug.Log(gameObject.name + " was released by left hand", this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient") && grabInteractable.isSelected)
        {
            IngredientMerger otherIngredient = other.GetComponent<IngredientMerger>();
            if (otherIngredient != null && otherIngredient.grabInteractable.isSelected)
            {
                if (rightHandInteractor != null && otherIngredient.leftHandInteractor != null)
                {
                    Debug.Log("Merging: " + gameObject.name + " (right hand) into " + other.gameObject.name + " (left hand)", this);
                    MakeChildOf(this, otherIngredient);
                }
                else if (leftHandInteractor != null && otherIngredient.rightHandInteractor != null)
                {
                    Debug.Log("Merging: " + other.gameObject.name + " (right hand) into " + gameObject.name + " (left hand)", this);
                    MakeChildOf(otherIngredient, this);
                }
            }
        }
    }

    private void MakeChildOf(IngredientMerger child, IngredientMerger parent)
    {
        child.transform.SetParent(parent.transform);
        Destroy(child.grabInteractable);
        Destroy(child.GetComponent<BoxCollider>());
        Destroy(child);
        Debug.Log(child.gameObject.name + " is now a child of " + parent.gameObject.name, this);
    }

      private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}