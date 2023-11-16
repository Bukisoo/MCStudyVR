using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientMerger : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor rightHandInteractor;
    private XRBaseInteractor leftHandInteractor;

    private void Awake()
    {
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
        // ... (rest of your OnTriggerEnter method)
    }

    private void MakeChildOf(IngredientMerger child, IngredientMerger parent)
    {
        // ... (rest of your MakeChildOf method)
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
