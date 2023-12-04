using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EasterEggScore : MonoBehaviour
{
    public TextMeshProUGUI EasterEggDisplay;
    private int Score = 0;

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Collider>().tag == "Ingredient"){
            Score++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EasterEggDisplay.text = Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        EasterEggDisplay.text = Score.ToString();
    }
}
