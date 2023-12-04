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

    private struct Order
    {
        public List<string> ingredients;
    }

    private List<Order> possibleOrders = new List<Order>()
    {
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Cheese", "Steak", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Cheese", "Steak", "Cheese", "Bottom Bun" } },
        new Order { ingredients = new List<string> { "Top Bun", "Lettuce", "Tomato", "Lettuce", "Bottom Bun" } }
    };

    private Order currentOrder;
    public TextMeshProUGUI orderDisplay;

    void Start()
    {
        originalX = transform.position.x;
        originalY = transform.position.y;
        originalZ = transform.position.z;
        currentOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
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

    void EvaluateBurger(GameObject burger)
    {
        List<string> burgerIngredients = new List<string>();
        Ingredient burgerIngredient = burger.GetComponent<Ingredient>();
        if (burgerIngredient != null)
        {
            burgerIngredients.Add(burgerIngredient.ingredientName);
        }

        foreach (Transform child in burger.transform)
        {
            Ingredient ingredient = child.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                burgerIngredients.Add(ingredient.ingredientName);
            }
        }

        string burgerComposition = "Burger composition: " + string.Join(", ", burgerIngredients);
        Debug.Log(burgerComposition);

        if (IsOrderCorrect(burgerIngredients, currentOrder))
        {
            Debug.Log("Burger accepted. Composition matches the order.");
            Drive = true;
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

    private void DisplayCurrentOrder()
    {
        //print the value of the script "Ingredient" to the textmeshpro one per line
        orderDisplay.text = "Order: \n";
        foreach (string ingredient in currentOrder.ingredients)
        {
            orderDisplay.text += ingredient + "\n";
        }
    }
}
