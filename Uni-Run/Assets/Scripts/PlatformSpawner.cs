using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점

    
    // 변수들을 초기화하고 사용할 발판들을 미리 생성
    void Start() {
        // count의 개수만큼 새로운 발판 배열 생성
        platforms = new GameObject[count];

        // count만큼 루프하면서 발판 생성
        for (int i = 0; i < count; i++)
        {
            // 프리팹을 원본으로 새 발판을 poolPosition에 복제 생성 후 배열에 할당
            platforms[i] = Instantiate(platformPrefab, poolPosition,Quaternion.identity);
        }
        
        // 마지막 배치 시점 초기화
        lastSpawnTime = 0f;

        // 다음번 배치까지 시간간격 초기화
        timeBetSpawn = 0f;
    }

    // 순서를 돌아가며 주기적으로 발판을 배치
    void Update() {
        // 캐릭터 사망시 작동 안함
        if (GameManager.instance.isGameover)
        {
            return;
        }
        
        // 게임 시작 안하면 작동 안함
        if (!GameManager.instance.GameStarted)
        {
            return;
        }

        // 마지막 배치 시점이후 timeBetSpawn이상 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 마지막 배치 시점을 현재로 변경
            lastSpawnTime = Time.time;
            
            // 다음배치까지의 시간 간격을 랜덤 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            
            // 배치의 위치도 랜덤 설정
            float yPos = Random.Range(-yMin, yMax);
            // Debug.LogWarning(yPos);
            
            // 사용할 현재 순번의 발판을 비활성화 직후 바로 활성화
            // 이때 OnEnable()이 실행
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);
            
            //현재 순번의 발판을 화면 오른쪽에 재배치
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            currentIndex++;

            // 마지막 순번 도달했다면 리셋
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}