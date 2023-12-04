using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // Import the new Input System namespace


public class IngredientMerger : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor rightHandInteractor;
    private XRBaseInteractor leftHandInteractor;
    
    private float currentPileHeight = 0f;
    private float yOffset = 0.1f;
    private float initialOffset = 0.1f; // Adjust this value to set the initial offset

    private BoxCollider triggerCollider;

    private void Awake()
    {
        currentPileHeight = initialOffset;
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component missing", this);
            return;
        }

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        // Add a secondary trigger collider
        triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
        UpdateTriggerColliderSize();
    }

// Update method using the new Input System
private void OnGrabbed(SelectEnterEventArgs arg)
    {
        //Debug.Log("Grabbed by interactor with tag: " + arg.interactor.tag, this);

        if (arg.interactor.CompareTag("RightHand"))
        {
            rightHandInteractor = arg.interactor;
            //Debug.Log(gameObject.name + " was grabbed by right hand", this);
        }
        else if (arg.interactor.CompareTag("LeftHand"))
        {
            leftHandInteractor = arg.interactor;
            //Debug.Log(gameObject.name + " was grabbed by left hand", this);

            // Set the collider's isTrigger property to true when grabbed by left hand
            SetColliderTriggerStatus(true);

            //log the value of the script "Ingredient" to the console of the ingredient that is grabbed
            Ingredient ingredient = GetComponent<Ingredient>();
            //Debug.Log("Ingredient name: (merger)" + ingredient.ingredientName);

        }
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        if (arg.interactor == rightHandInteractor)
        {
            rightHandInteractor = null;
            //Debug.Log(gameObject.name + " was released by right hand", this);
        }
        else if (arg.interactor == leftHandInteractor)
        {
            leftHandInteractor = null;
            //Debug.Log(gameObject.name + " was released by left hand", this);

            // Revert the collider's isTrigger property to false when released by left hand
            SetColliderTriggerStatus(false);
        }
    }

    private void SetColliderTriggerStatus(bool isTrigger)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = isTrigger;
        }
    }


private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Ingredient") && grabInteractable.isSelected)
    {
        IngredientMerger otherIngredient = other.GetComponent<IngredientMerger>();
        if (otherIngredient != null && otherIngredient.grabInteractable.isSelected)
        {
            // If this object is grabbed by the left hand and the other object is grabbed by the right hand
            if (leftHandInteractor != null && otherIngredient.rightHandInteractor != null)
            {
                MakeChildOf(otherIngredient, this);
            }
            // If this object is grabbed by the right hand and the other object is grabbed by the left hand
            else if (rightHandInteractor != null && otherIngredient.leftHandInteractor != null)
            {
                MakeChildOf(this, otherIngredient);
            }
        }
    }

}


    private void MakeChildOf(IngredientMerger child, IngredientMerger parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + child.gameObject.name);
        if (prefab == null)
        {
            Debug.LogError("No prefab found for name: " + child.gameObject.name + ". Path: Prefabs/" + child.gameObject.name, this);
            return;
        }

        GameObject newChild = Instantiate(prefab, parent.transform);
        newChild.name = prefab.name;

        Vector3 originalScale = child.transform.localScale;
        Vector3 inverseParentScale = new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z);
        newChild.transform.localScale = Vector3.Scale(originalScale, inverseParentScale);
        newChild.transform.localPosition = new Vector3(0, currentPileHeight, 0);
        newChild.transform.localRotation = Quaternion.identity;

        currentPileHeight += yOffset;

        Rigidbody rigidbody = newChild.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }

        XRGrabInteractable grabInteractable = newChild.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        BoxCollider boxCollider = newChild.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }

        Destroy(child.gameObject);
        //Debug.Log(newChild.name + " instantiated and set as child of " + parent.gameObject.name, this);

        UpdateTriggerColliderSize();

        //list of all the ingredients in the burger, you can get the ingredient name by using ingredient.ingredientName on each prefab
        //print the value of the script "Ingredient" to the console
        Ingredient ingredient = newChild.GetComponent<Ingredient>();
        //Debug.Log("Ingredient name: " + ingredient.ingredientName);

        
    }

    private void UpdateTriggerColliderSize()
    {
        if (triggerCollider != null)
        {
            // Adjust the size and position of the trigger collider as needed
            // Here, I'm just using an example. You should set this according to your requirements.
            triggerCollider.size = new Vector3(1, 1, 1);
            triggerCollider.center = new Vector3(0, 0, 0);
        }
    }

}
