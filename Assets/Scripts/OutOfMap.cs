using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfMap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile")) Destroy(collision.gameObject);
    }
}
