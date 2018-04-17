using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inRange : MonoBehaviour
{


    public GameObject spawner;
 

    // Use this for initialization
    void Start()
    {

    }




    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
           spawner.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            spawner.SetActive(true);
    }
}
