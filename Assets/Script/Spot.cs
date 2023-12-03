using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Light>().color = Color.Lerp(Color.blue, Color.red, Mathf.PingPong(Time.time, 1));
    }
}
