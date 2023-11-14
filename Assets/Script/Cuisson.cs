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
        }
        if(other.gameObject.tag == "Poubelle"){
            Debug.Log("test");
            Destroy(this.gameObject);
        }
}

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Grill"){
            CuissonViande = false;
    }
}

    // Update is called once per frame
    void Update()
    {
        if(CuissonViande == true){
            TempsCuisson++;
            if(TempsCuisson < 1800)
            {
                Debug.Log("La viande cuit");
            }
            if(TempsCuisson>= 1800 && TempsCuisson< 3600){
                Debug.Log("La viande est cuite");
                this.GetComponent<Renderer>().material = MatCuit;
            }
            if(TempsCuisson>= 3600){
                Debug.Log("La viande est cramé");
                this.GetComponent<Renderer>().material = MatCrame;
            }
        }
    }
}