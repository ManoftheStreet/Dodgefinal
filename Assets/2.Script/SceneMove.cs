using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTransition
{


    public class SceneMove : MonoBehaviour
    {
        public string introScene;
        public string gameScene;
        public GameObject tutorial;
        public AudioSource[] playSounds;
        public TransitionSettings transition;
        public float startDelay;

        public GameObject stopPanel;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (stopPanel.activeInHierarchy) // 만약 stopPanel이 이미 활성화되어 있다면
                {
                    ResumeGame();  // 게임을 다시 시작합니다.
                }
                else  // stopPanel이 비활성화되어 있다면
                {
                    PauseGame();  // 게임을 일시정지합니다.
                }
            }
        }

        public void GameStart()
        {
            StartCoroutine(GameSceneLoad());
        }


        private IEnumerator GameSceneLoad()
        {
            AudioSource selectedSound = playSounds[Random.Range(0, playSounds.Length)];
            selectedSound.Play();
            TransitionManager.Instance().Transition(gameScene, transition, startDelay);
            yield return new WaitForSeconds(selectedSound.clip.length);  // 선택한 소리의 길이만큼 대기
            //SceneManager.LoadScene(gameScene);
            //TransitionManager.Instance().Transition(gameScene, transition, startDelay);
        }

        public void MainMenu()
        {
            StartCoroutine(IntroSceneLoad());
        }

        private IEnumerator IntroSceneLoad()
        {
            AudioSource selectedSound = playSounds[Random.Range(0, playSounds.Length)];
            selectedSound.Play();
            TransitionManager.Instance().Transition(introScene, transition, startDelay);
            yield return new WaitForSeconds(selectedSound.clip.length);  // 선택한 소리의 길이만큼 대기
            //SceneManager.LoadScene(introScene);
            //TransitionManager.Instance().Transition(introScene, transition, startDelay);
        }
        public void PauseGame()
        {
            Time.timeScale = 0f;  // 게임의 일시정지
            stopPanel.SetActive(true);  // stopPanel의 활성화
        }
        public void ResumeGame()
        {
            Time.timeScale = 1f;  // 게임의 재개
            stopPanel.SetActive(false);  // stopPanel의 비활성화
        }

        public void EndGame()
        {
            Application.Quit();
        }
    }
}