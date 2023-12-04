using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poubelle : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Poubelle"){
            //Debug.Log("test");
            Destroy(this.gameObject);
        }
    }
}
