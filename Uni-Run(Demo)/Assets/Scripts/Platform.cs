using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물 오브젝트들
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가

    // 컴포넌트가 활성화될때 마다 매번 실행되는 발판 리셋 메서드
    private void OnEnable() {
        stepped = false;    // 밟힘 상태 리셋
        
        // 장애물 개수만큼 루프
        for(int i = 0; i < obstacles.Length; i++)
        {
            // 1/3의 확률로 장애물 활성
            if (Random.Range(0, 3) == 0)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
    }

    // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
    void OnCollisionEnter2D(Collision2D collision) {
        // 충돌한 상대방의 태그가 player이고 이전 플레이어 캐릭터가 밟지 않았다면
        if (collision.gameObject.tag == "Player" && !stepped)
        {
            stepped = true;
            GameManager.instance.AddScore(1);
        }
    }
}