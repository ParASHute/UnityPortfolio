using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour {
    public float speed = 10f; // 이동 속도

    private void Update() {
        if (!GameManager.instance.isGameover && GameManager.instance.GameStarted)
        {
            transform.Translate(Vector2.left * (speed * Time.deltaTime));
            /*
                Vector2.left == (-1.0.0)
                Vector2.left * speed == (-speed,0,0)
             */
        }
    }
}