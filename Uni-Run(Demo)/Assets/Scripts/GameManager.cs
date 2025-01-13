using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 정적(static) 변수
    // 메니저 오브잭트는 무한하게 존제할수 있지만 그 모든 메니저 오브젝트의 instance는 오롯이 단 1개만이 존제

    public bool isGameover = false; // 게임 오버 상태
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤 디자인 패턴 구성
    void Awake() {
        // 싱글톤 변수 instance가 비어있다면
        if (instance == null)
        {
            // 자기 자신을 할당
            instance = this;
        }
        else
        {
            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다면 자신의 게임 오브젝트를 Destroy
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
            
            // 위 else문 안에서 무한하게 생성된 많은 메니저 오브젝트들이 자기 스스로를 지우게 됨
            // 결과적으로 메니저 오브젝트 또한 1가지만 생성
        }
    }

    void Update() {
        if(isGameover && Input.GetMouseButtonDown(0))
        {
            // 게임 오버 상태에서 마우스 왼쪽 버튼 클릭 == 현재신 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isGameover && Input.GetKey(KeyCode.Escape))
        {
            GameEnd();
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
    
    private void GameEnd() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}