using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float physicalDamage;
    private float fireDamage;
    private bool bPropertiesSet = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bPropertiesSet)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    public void SetProperties(float speed, float physicalDamage, float fireDamage, Vector3 scale)
    {
        this.physicalDamage = physicalDamage;
        this.fireDamage = fireDamage;
        if (scale.x < 0) this.speed = -speed;
        else this.speed = speed;

        bPropertiesSet = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(physicalDamage, fireDamage);

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Barrier"))
        {
            Destroy(gameObject);
        }
    }
}
