using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireBall : MonoBehaviour
{
    public float speed = 8f;
   

    private Rigidbody rb;
    public Transform target;
    NavMeshAgent nav;
    Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();

        float randomTime = Random.Range(3f, 5f);

        Destroy(gameObject, randomTime);
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
    void Update()
    {
        if (target != null && !player.isDead)
        {
            nav.SetDestination(target.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}