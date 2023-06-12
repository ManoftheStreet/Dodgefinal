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
        Quaternion rotation = Quaternion.Euler(0f, 30f, 0f); // 전방 45도 회전 각도 생성
        Vector3 forward45 = rotation * transform.forward; // 전방 45도 회전된 벡터 계산
        rb.velocity = forward45 * speed; // 전방 45도로 움직임

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

