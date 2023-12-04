using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoingForward : MonoBehaviour
{
    public float speed = 1.0f;
    private float originalX;
    private float originalY;
    private float originalZ;
    private bool Drive = true;

    // Define an order structure
    private struct Order
    {
        public List<string> ingredients;
    }

    // List of possible orders
    private List<Order> possibleOrders = new List<Order>()
    {
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Cheese", "Cooked Steak", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Cheese", "Cooked Steak", "Cheese", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Lettuce", "Bottom Bun" } }
    };

    // Current order
    private Order currentOrder;

    // Public TextMeshPro - Test (UI) for displaying the order
    public TextMeshProUGUI orderDisplay;

    void Start()
    {
        originalX = transform.position.x;
        originalY = transform.position.y;
        originalZ = transform.position.z;

        // Select a random order
        currentOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];

        // Display the current order
        DisplayCurrentOrder();
    }

    void Update()
    {
        if (Drive)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "resetCar")
        {
            Debug.Log("Car Reset");
            transform.position = new Vector3(originalX, originalY, originalZ);
        }
        else if (other.gameObject.tag == "Drive")
        {
            Drive = false;
        }
        else if (other.gameObject.tag == "Ingredient")
        {
            EvaluateBurger(other.gameObject);
        }
    }

    // Evaluate the received burger
void EvaluateBurger(GameObject burger)
{
    List<string> burgerIngredients = new List<string>();

    foreach (Transform child in burger.transform)
    {
        Ingredient ingredient = child.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            burgerIngredients.Add(ingredient.ingredientName);
        }
    }

    Debug.Log("Burger Composition: " + string.Join(", ", burgerIngredients));
    

    // Display burger composition in the console
    Debug.Log("Burger Composition: " + string.Join(", ", burgerIngredients));

    if (IsOrderCorrect(burgerIngredients, currentOrder))
    {
        Debug.Log("Burger accepted.");
        Drive = true; // Resume driving
    }
    else
    {
        Debug.Log("Burger rejected.");
        // Additional logic for rejected burger
    }
}

    // Check if the burger's ingredients match the current order (regardless of order)
    private bool IsOrderCorrect(List<string> burgerIngredients, Order order)
    {
        foreach (string ingredient in order.ingredients)
        {
            if (!burgerIngredients.Contains(ingredient))
                return false;
        }

        return true;
    }

    // Display the current order on the TextMesh
    private void DisplayCurrentOrder()
    {
        string orderText = "Order: ";
        foreach (string ingredient in currentOrder.ingredients)
        {
            orderText += "\n" + ingredient;
        }

        orderDisplay.text = orderText;
    }
}
