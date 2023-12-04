using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoingForward : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public float speed = 1.0f;
    private float originalX;
    private float originalY;
    private float originalZ;
    private int score = 0;
    private bool Drive = true;

    private struct Order
    {
        public List<string> ingredients;
    }

    private List<Order> possibleOrders = new List<Order>()
    {
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Cheese", "Steak", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Cheese", "Steak", "Cheese", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Steak", "Bottom Bun" } }
    };

    private Order currentOrder;
    public TextMeshProUGUI orderDisplay;

    void Start()
    {
        originalX = transform.position.x;
        originalY = transform.position.y;
        originalZ = transform.position.z;

        scoreDisplay.text = score.ToString();
        SelectNewOrder();
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
            transform.position = new Vector3(originalX, originalY, originalZ);
            score++;
            scoreDisplay.text = score.ToString();
        }
        else if (other.gameObject.tag == "Drive")
        {
            Drive = false;
        }
        else if (other.gameObject.tag == "Ingredient" && other.gameObject.transform.childCount > 0)
        {
            EvaluateBurger(other.gameObject);
        }
    }

    void EvaluateBurger(GameObject burger)
    {
    List<string> burgerIngredients = new List<string>();

    // Check if the burger itself has an Ingredient component
    Ingredient burgerIngredient = burger.GetComponent<Ingredient>();
    if (burgerIngredient != null)
    {
        burgerIngredients.Add(burgerIngredient.ingredientName);
    }

    // Iterate through each child and add their ingredient names
    foreach (Transform child in burger.transform)
    {
        Ingredient ingredient = child.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            burgerIngredients.Add(ingredient.ingredientName);
        }
    }

    // Remove duplicate ingredient names
    //burgerIngredients = new HashSet<string>(burgerIngredients).ToList();

    // Create a string to represent the burger composition
    string burgerComposition = "Burger Composition: " + string.Join(", ", burgerIngredients);
    Debug.Log(burgerComposition);

    // Check if the burger's ingredients match the current order
    if (IsOrderCorrect(burgerIngredients, currentOrder))
        {
            Debug.Log("Burger accepted. Composition matches the order.");
            Drive = true;
            SelectNewOrder(); // Select a new order when the current one is accepted
        }
        else
        {
            Debug.Log("Burger rejected. Composition does not match the order.");
        }
}
    private bool IsOrderCorrect(List<string> burgerIngredients, Order order)
    {
        foreach (string ingredient in order.ingredients)
        {
            if (!burgerIngredients.Contains(ingredient))
                return false;
        }
        return true;
    }
    
    private void SelectNewOrder()
    {
        currentOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
        DisplayCurrentOrder();
    }

    private void DisplayCurrentOrder()
    {
        orderDisplay.text = "Order: \n";
        foreach (string ingredient in currentOrder.ingredients)
        {
            orderDisplay.text += ingredient + "\n";
        }
    }
}
