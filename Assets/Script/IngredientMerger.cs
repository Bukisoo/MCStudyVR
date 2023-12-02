using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientMerger : MonoBehaviour
{

    // Add a field to reference the prefab
    [SerializeField] private GameObject ingredientPrefab;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor rightHandInteractor;
    private XRBaseInteractor leftHandInteractor;
    
     // Variable to track the height of the pile
    private float currentPileHeight = 0f;
    private float yOffset = 0.1f; // Adjust this value as needed

    private void Awake()
    {
        //Debug.Log("IngredientMerger Awake called", this);
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
    // Add a debug statement to log the tag of the interactor
    Debug.Log("Grabbed by interactor with tag: " + arg.interactor.tag, this);

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
    // Dynamically load the prefab based on the child's name
    GameObject prefab = Resources.Load<GameObject>("Prefabs/" + child.gameObject.name);

    if (prefab == null)
    {
        Debug.LogError("No prefab found for name: " + child.gameObject.name + ". Path: Prefabs/" + child.gameObject.name, this);
        return;
    }
    // Instantiate a new object from the loaded prefab
    GameObject newChild = Instantiate(prefab, parent.transform);

    // Change the name of the instantiated object to match the prefab name
    newChild.name = prefab.name;

    // Store original scale
    Vector3 originalScale = child.transform.localScale;

    // Calculate inverse scale of parent
    Vector3 inverseParentScale = new Vector3(
        1 / parent.transform.localScale.x,
        1 / parent.transform.localScale.y,
        1 / parent.transform.localScale.z
    );

    // Apply inverse scale to maintain original scale
    newChild.transform.localScale = Vector3.Scale(originalScale, inverseParentScale);

    // Set local position and rotation
    newChild.transform.localPosition = Vector3.zero;
    newChild.transform.localRotation = Quaternion.identity;

    // Offset position along Y-axis based on the current pile height
    newChild.transform.localPosition = new Vector3(0, currentPileHeight, 0);
    newChild.transform.localRotation = Quaternion.identity;

    // Increment the pile height for the next child
    currentPileHeight += yOffset;

    // Disable Rigidbody
    var rigidbody = newChild.GetComponent<Rigidbody>();
    if (rigidbody != null)
    {
        rigidbody.isKinematic = true;
    }

    // Disable XRGrabInteractable script
    var grabInteractable = newChild.GetComponent<XRGrabInteractable>();
    if (grabInteractable != null)
    {
        grabInteractable.enabled = false;
    }

    // Disable and remove Box Collider
    var boxCollider = newChild.GetComponent<BoxCollider>();
    if (boxCollider != null)
    {
        Destroy(boxCollider);
    }

    // Destroy the original child object
    Destroy(child.gameObject);

    Debug.Log(newChild.name + " instantiated and set as child of " + parent.gameObject.name, this);
}

}