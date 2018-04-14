using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activation_radius : MonoBehaviour
{

    public GameObject[] objs;

    public bool stay_on;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < objs.Length; ++i)
            {
                objs[i].SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !stay_on)
        {
            for (int i = 0; i < objs.Length; ++i)
            {
                if (objs[i] != null) objs[i].SetActive(false);
            }
        }
    }
}
