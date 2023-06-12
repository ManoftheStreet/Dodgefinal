using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public GameObject prefab;
    public AudioSource sound;

    public float rateMin = 0.5f;
    public float rateMax = 3f;

    Transform target;
    float rate = 0.1f;
    float timeAfterSpawn;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;

        sound = GetComponent<AudioSource>();
        rate = Random.Range(rateMin, rateMax);
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

                GameObject instantFireballA = Instantiate(prefab, transform.position, transform.rotation);
                FireBall fireballA = instantFireballA.GetComponent<FireBall>();
                fireballA.target = target;

                timeAfterSpawn = 0;
                rate = Random.Range(rateMin, rateMax);
            }
        }
    }
}
