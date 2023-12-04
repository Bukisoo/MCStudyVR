using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuisson : MonoBehaviour
{
    public Material MatCuit;
    public Material MatCrame;
    private bool CuissonViande;
    private int TempsCuisson = 0;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Grill"){
            CuissonViande = true;
            GetComponent <ParticleSystem>().Play();
        }
}

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Grill"){
            CuissonViande = false;
            GetComponent <ParticleSystem>().Stop();
        }
    }

    void Start()
    {
        GetComponent <ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(CuissonViande == true){
            TempsCuisson++;
            if(TempsCuisson>= 600 && TempsCuisson< 1800){
                // Debug.Log("La viande est cuite");
                this.GetComponent<Renderer>().material = MatCuit;
                //change the name of the object to "Steak" in the script "Ingredient"
                Ingredient ingredient = GetComponent<Ingredient>();
                ingredient.ingredientName = "Steak";
                Debug.Log(ingredient.ingredientName);
                
            }
            if(TempsCuisson>= 1800){
                // Debug.Log("La viande est cramé");
                this.GetComponent<Renderer>().material = MatCrame;
                this.GetComponent<ParticleSystemRenderer>().material = MatCrame;
                //change the name of the object to "Overcooked Steak" in the script "Ingredient"
                Ingredient ingredient = GetComponent<Ingredient>();
                ingredient.ingredientName = "Overcooked Steak";
                Debug.Log(ingredient.ingredientName);
            }

        }
    }
}