using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingForward : MonoBehaviour
{
    public float speed = 1.0f;
    private float originalX;
    private float originalY;
    private float originalZ;
    private bool Drive = true;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "resetCar"){
            Debug.Log("test");
            // Reset the position of the object
            transform.position = new Vector3(originalX, originalY, originalZ);
        }
        if(other.gameObject.tag == "Drive"){
            //stop car
            Drive = false;
        }
        if (other.GetComponent<Collider>().tag == "Ingredient")
        {
            Drive = true;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Save the original position of the object
        originalX = transform.position.x;
        originalY = transform.position.y;
        originalZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Drive == true){
            MoveForward();
        }
    }

    void MoveForward()
    {
        // Move the object on positive X axis
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
}

