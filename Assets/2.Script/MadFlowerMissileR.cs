using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadFlowerMissileR : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Quaternion rotation = Quaternion.Euler(0f, 30f, 0f); // ���� 45�� ȸ�� ���� ����
        Vector3 forward45 = rotation * transform.forward; // ���� 45�� ȸ���� ���� ���
        rb.velocity = forward45 * speed; // ���� 45���� ������

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

