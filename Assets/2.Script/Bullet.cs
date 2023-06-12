using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.tag == "Player")
         {
             Player pc = other.GetComponent<Player>();
             if (pc != null)
             {
                 pc.Die();
             }
         }*/
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
