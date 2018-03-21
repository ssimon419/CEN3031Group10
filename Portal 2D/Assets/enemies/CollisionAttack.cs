using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAttack : MonoBehaviour {
    public GameObject hunted;

    public int playerHealth = 100;

    public int attackDamage;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == hunted)
        {
            playerHealth -= attackDamage;
        }
        if (playerHealth <= 0)
        {
            Destroy(col.gameObject);
        }
    }
}
