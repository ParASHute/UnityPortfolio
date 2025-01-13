using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour
{
    private float width; // 배경의 가로 길이

    // Start()처럼 처음에 시작하는 부분
    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
        // 가로 길이를 측정하는 처리
    }

    // 매 프레임마다 실행 되기 때문에 이를 이용해 현재 위치를 계속 파악할 예정
    private void Update()
    {
        if (GameManager.instance.GameStarted)
        {
            // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
            if (transform.position.x <= -width)
            {
                Reposition();
            }
        }
        
    }

    // 위치를 리셋하는 메서드, Update에서 실행
    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }

    
}