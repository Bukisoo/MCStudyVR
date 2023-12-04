using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{

    public GameObject BasketHop;
    public AudioSource AudioEasterEgg;
    public AudioSource McDoAudio;
    public AudioSource AudioGens;
    public GameObject MainLight;
    public GameObject SpotLight;
    private bool EasterEggState = false;
    private int TempsEasterEgg = 0;

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Collider>().tag == "Ingredient"){
            BasketHop.gameObject.SetActive(true);
            AudioEasterEgg.gameObject.SetActive(true);
            McDoAudio.gameObject.SetActive(false);
            AudioGens.gameObject.SetActive(false);
            MainLight.gameObject.SetActive(false);
            SpotLight.gameObject.SetActive(true);
            EasterEggState = true;
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
        if(EasterEggState == true){
            TempsEasterEgg++;
            if(TempsEasterEgg>= 600){
                BasketHop.gameObject.SetActive(false);
                AudioEasterEgg.gameObject.SetActive(false);
                McDoAudio.gameObject.SetActive(true);
                AudioGens.gameObject.SetActive(true);
                MainLight.gameObject.SetActive(true);
                SpotLight.gameObject.SetActive(false);
                EasterEggState = false;
                TempsEasterEgg = 0;
            }
        }
    }
}
