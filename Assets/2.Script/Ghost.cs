using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject prefab;
    public float rateMin = 0.7f;
    public float rateMax = 3f;
    public AudioSource sound;

    Transform target;
    float rate;
    float timeAfterSpawn;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;
        rate = Random.Range(rateMin, rateMax);
        sound = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            target = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            timeAfterSpawn += Time.deltaTime;

            if (timeAfterSpawn > rate && !player.isDead)
            {
                sound.Play();
                Instantiate(prefab, transform.position + new Vector3(0, 1, 0), transform.rotation);

                timeAfterSpawn = 0;
                rate = Random.Range(rateMin, rateMax);
            }
        }
    }
}
