using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{

    public GameObject BasketHop;

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Collider>().tag == "Ingredient"){
            Debug.Log("test");
            BasketHop.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BasketHop.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
