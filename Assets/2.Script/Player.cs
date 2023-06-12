using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;
    public GameObject hideSkillButton;
    public UnityEngine.UI.Image hideskillImage;
    private bool isHideSkill = false;
    public float skillTimes;

    public Rigidbody rb;
    public CapsuleCollider cl;
    public float speed = 8f;
    public float dodtime;
    public GameManager manager;
    public GameObject trailEffect;

    public AudioSource[] dodgeSounds;
    public AudioSource[] dieSounds;
    public AudioSource footStep;
    public AudioSource hitSound;


    bool isDodge;
    bool dDown;
    public bool isDead;


    float dodgeTime;
    Animator anim;

    Vector3 dodVec;
    GameObject trail;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cl = GetComponent<CapsuleCollider>();
        footStep = GetComponent<AudioSource>();
        hideSkillButton.SetActive(false);
        isDead = false;
        dodgeTime = 0.0f;
    }

    void Update()
    {
        Move();
        Dodge();
        Turn ();
        HideSkillChk();
    }
    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        float xJoyInput = joy.Horizontal;
        float zJoyInput = joy.Vertical;


        float xSpeed = (xInput + xJoyInput) * speed;
        float zSpeed = (zInput + zJoyInput) * speed;
        if (!isDead)
        {
            if (isDodge) rb.velocity = dodVec;

            Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);

            newVelocity = newVelocity.normalized * speed;
            rb.velocity = newVelocity;

            anim.SetBool("doRun", rb.velocity != Vector3.zero);
            if (rb.velocity != Vector3.zero)
            {
                footStep.enabled = true;
            }
            else { StartCoroutine(footStepoff()); }
        }
    }
    IEnumerator footStepoff()
    {
        yield return new WaitForSeconds(0.1f);
        footStep.enabled = false;
    }
    public void Turn()
    {

        if (rb.velocity != Vector3.zero && rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.LookAt(transform.position + rb.velocity);
        }

    }
    public void Dodge() //회피
    {
        dDown = Input.GetButtonDown("Dodge");

        //dodgeTime -= Time.deltaTime;

        if (dDown && !isDead && !isDodge && dodgeTime < 0.01f)
        {
            isDodge = true;
            AudioSource selectedSound = dodgeSounds[Random.Range(0, dodgeSounds.Length)];
            selectedSound.Play();
            anim.SetTrigger("doDodge");
            trailEffect.SetActive(true);
            speed *= 2.0f;
            
            hideSkillButton.SetActive(true);
            dodgeTime = skillTimes;
            isHideSkill = true;
            StartCoroutine(skillTimeChk());

            Invoke("DodgeOut", dodtime);
        }
    }

    public void Dodge2() //회피
    {


        if (!isDead && !isDodge && dodgeTime < 0.01f)
        {
            isDodge = true;
            AudioSource selectedSound = dodgeSounds[Random.Range(0, dodgeSounds.Length)];
            selectedSound.Play();
            anim.SetTrigger("doDodge");
            trailEffect.SetActive(true);
            speed *= 2.0f;

            hideSkillButton.SetActive(true);
            dodgeTime = skillTimes;
            isHideSkill = true;
            StartCoroutine(skillTimeChk());

            Invoke("DodgeOut", dodtime);
        }
    }
    void DodgeOut() //회피종료
    {
        speed *= 0.5f;
        StartCoroutine(TrailOff());
        isDodge = false;
    }

    private void HideSkillChk()
    {
        if (isHideSkill)
        {
            StartCoroutine(skillTimeChk());
        }
    }
    IEnumerator TrailOff()
    {
        yield return new WaitForSeconds(0.28f);
        trailEffect.SetActive(false);
    }
    IEnumerator skillTimeChk()
    {
        yield return null;  // 다음 프레임을 기다립니다.

        if (dodgeTime > 0.0f) // dodgeTime이 2.0f 미만일 때만 증가
        {
            dodgeTime -= Time.deltaTime;

            if (dodgeTime < 0.0f) // 만약 dodgeTime이 2.0f 이상이라면
            {
                dodgeTime = 0.0f; // dodgeTime을 2.0f로 설정하고
                hideSkillButton.SetActive(false); // 버튼을 비활성화합니다.
                isHideSkill = false;
            }

            float time = dodgeTime / skillTimes;
            hideskillImage.fillAmount = time;
        }
    }
    void FreezeRotation()//뜻하지 않은 물리력에 의한 회전을 막음
    {
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotation();
  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy"&& !isDead)
        {
            Die();
        }
        
    }
    public void Die()
    {
        //gameObject.SetActive(false);
        if (!isDodge)
        {
            hitSound.Play();
            anim.SetTrigger("doDie");
            rb.velocity = Vector3.zero;
            isDead = true;
            AudioSource selectedSound = dieSounds[Random.Range(0, dieSounds.Length)];
            selectedSound.Play();
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.GameOver();
            joy.gameObject.SetActive(false);
            GameObject DodgeButton = GameObject.Find("DodgeButton"); // 이름으로 게임 오브젝트 찾기
            if (DodgeButton != null)
            { // 찾았을 경우
                DodgeButton.SetActive(false); // DodgeButton을 비활성화
            }
            footStep.enabled = false;
            //manager.GameOver();
            //Destroy(gameObject);
        }
    }
}
