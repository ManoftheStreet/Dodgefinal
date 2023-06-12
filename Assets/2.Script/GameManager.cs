using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int Ghost;
    public int MadFlower;
    public int Dragon;
    int ran;

    public GameObject gameoverText;
    public GameObject winText;
    public Text timeText;
    public Text recordText;
    public Text gotimeText;
    float surviveTime;
    bool isGameOver;

    Player player;
    public AudioSource endSound;
    public AudioSource gameSound;
    public AudioSource winSound;


    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;
    
  
    void Awake()
    {
        surviveTime = -1.0f;
        isGameOver = false; 
        enemyList = new List<int>();
        player = FindObjectOfType<Player>();
        gameSound.Play();
        StartCoroutine(InBattle());
    }
    void Update()
    {
        if (!isGameOver)
        {
            surviveTime += Time.deltaTime;
            int milliseconds = (int)((surviveTime * 100) % 100);
            int seconds = (int)(surviveTime % 60);
            int minutes = (int)((surviveTime / 60) % 60);

            timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
            if (gameSound.clip != null && surviveTime >= gameSound.clip.length)
            {
                Win();
            }
           
        }
        /*else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("GameScene");
            }
        }*/
    }

    public void Win()
    {
        isGameOver = true;
        player.gameObject.SetActive(false);
        winText.SetActive(true);
        timeText.gameObject.SetActive(false);
        gameSound.Stop();
        winSound.Play();
        GameObject joypad = GameObject.Find("FloatingJoystick"); // 이름으로 게임 오브젝트 찾기
        if (joypad != null)
        { // 찾았을 경우
            joypad.SetActive(false); // DodgeButton을 비활성화
        }
        GameObject DodgeButton = GameObject.Find("DodgeButton"); // 이름으로 게임 오브젝트 찾기
        if (DodgeButton != null)
        { // 찾았을 경우
            DodgeButton.SetActive(false); // DodgeButton을 비활성화
        }

        float bestTime = PlayerPrefs.GetFloat("Best Time");
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("Best Time", bestTime);
        }
        int milliseconds = (int)((bestTime * 100) % 100);
        int seconds = (int)(bestTime % 60);
        int minutes = (int)((bestTime / 60) % 60);
        int GOmilliseconds = (int)((surviveTime * 100) % 100);
        int GOseconds = (int)(surviveTime % 60);
        int GOminutes = (int)((surviveTime / 60) % 60);

        timeText.text = "도주 시간: " + string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);

        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        recordText.text = "최고 기록: " + string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        gotimeText.text = "도주 시간" + string.Format("{0:D2}:{1:D2}:{2:D2}", GOminutes, GOseconds, GOmilliseconds);
    }
    public void GameOver()
    {
        isGameOver = true;
        gameoverText.SetActive(true);
        timeText.gameObject.SetActive(false);
        gameSound.Stop();
        endSound.Play();

        float bestTime = PlayerPrefs.GetFloat("Best Time");
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("Best Time", bestTime);
        }
        int milliseconds = (int)((bestTime * 100) % 100);
        int seconds = (int)(bestTime % 60);
        int minutes = (int)((bestTime / 60) % 60);
        int GOmilliseconds = (int)((surviveTime * 100) % 100);
        int GOseconds = (int)(surviveTime % 60);
        int GOminutes = (int)((surviveTime / 60) % 60);

        timeText.text = "도주 시간: " + string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);

        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        recordText.text = "최고 기록: " + string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        gotimeText.text = "도주 시간" + string.Format("{0:D2}:{1:D2}:{2:D2}", GOminutes, GOseconds, GOmilliseconds);
    }
    /*IEnumerator InBattle()
    {
        if (player == null)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            //for (int index = 0; index < 300; index++)
            while (!player.isDead)
            {
                ran = UnityEngine.Random.Range(0, 10);
                enemyList.Add(ran);
            
                int ranZone = UnityEngine.Random.Range(0, 25);

                float randomspot = Random.Range(0.1f, 3.0f);

                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position + new Vector3(randomspot,0, randomspot), enemyZones[ranZone].rotation);
                float randomDestroy = Random.Range(5f, 10f);
                Destroy(instantEnemy, randomDestroy);
                enemyList.RemoveAt(0);
                float respawnRate = Random.Range(1f, 2f);
                yield return new WaitForSeconds(respawnRate);

            }
        }
    }*/
    IEnumerator InBattle()
    {
        if (player == null)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            while (!player.isDead)
            {
                // 시간에 따라 난이도를 증가시키는 로직
                float difficulty = Mathf.Clamp(surviveTime / 77f, 0f, 1f);
                ran = GenerateEnemyBasedOnDifficulty(difficulty);

                enemyList.Add(ran);
                int ranZone = UnityEngine.Random.Range(0, 25);
                float randomspot = Random.Range(0.1f, 3.0f);

                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position + new Vector3(randomspot, 0, randomspot), enemyZones[ranZone].rotation);
                float randomDestroy = Random.Range(6f, 8f);
                Destroy(instantEnemy, randomDestroy);
                enemyList.RemoveAt(0);
                float respawnRate = Random.Range(2f, 2.5f);
                yield return new WaitForSeconds(respawnRate);
            }
        }
    }

    int GenerateEnemyBasedOnDifficulty(float difficulty)
    {
        // 0.0f ~ 0.33f : 쉬운 적
        // 0.34f ~ 0.66f : 중간 난이도 적
        // 0.67f ~ 1.0f : 어려운 적
        if (difficulty < 0.10f)
        {
            return UnityEngine.Random.Range(0, 5);
        }
        else if (difficulty < 0.20f)
        {
            return UnityEngine.Random.Range(0, 7);
        }
        else if (difficulty < 0.30f)
        {
            return UnityEngine.Random.Range(0, 9);
        }
        else if (difficulty < 0.40f)
        {
            return UnityEngine.Random.Range(0, 10);
        }
        else if (difficulty < 0.50f)
        {
            return UnityEngine.Random.Range(1, 10);
        }
        else if (difficulty < 0.60f)
        {
            return UnityEngine.Random.Range(2, 10);
        }
        else if (difficulty < 0.70f)
        {
            return UnityEngine.Random.Range(3, 10);
        }
        else if (difficulty < 0.80f)
        {
            return UnityEngine.Random.Range(4, 10);
        }
        else
        {
            return UnityEngine.Random.Range(5, 10);
        }
    }
}
