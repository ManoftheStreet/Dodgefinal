using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadFlower : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefabR;
    public GameObject prefabL;
    public AudioSource sound;

    public float rateMin = 0.6f;
    public float rateMax = 1f;
    float fireRate = 0.3f;
    

    Transform target;
    Animator anim;
    float rate;
    float timeAfterSpawn;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;

        sound = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            target = player.transform;
        }

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            timeAfterSpawn += Time.deltaTime;

            if (timeAfterSpawn > fireRate && !player.isDead)
            {
                sound.Play();
                anim.SetTrigger("doAttack");

                Instantiate(prefab1, transform.position + new Vector3(0, 1, 0), transform.rotation);
                Instantiate(prefabR, transform.position + new Vector3(0, 1, 0), transform.rotation);
                Instantiate(prefabL, transform.position + new Vector3(0, 1, 0), transform.rotation);

                timeAfterSpawn = 0;
                fireRate = Random.Range(rateMin, rateMax);
            }
        }
    }
}
