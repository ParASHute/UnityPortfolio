using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   // For Animatiom
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   // Player State
   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   // Player Game Object Components
   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
   
// 초기화(생성자)
   private void Start() {
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
   }

// 사용자 입력을 감지하고 점프하는 처리
   private void Update() {
      if (isDead)
      {
         // 플레이어 사망시 종료
         return;
      }
      
      /*
         GetMouseButtonDown : 늘렀을때
         GetMouseButton : 누르고 있을때
         GetMouseButtonUp : 땠을때
         마우스 클릭은 0,1,2로 받는데
         0 : 좌클릭
         1 : 우클릭
         2 : 휠버튼
      */
      if (Input.GetMouseButtonDown(0) && jumpCount < 2) // 마우스 좌클릭 입력 받는 조건문
      {
         jumpCount++;   // 점프 횟수 증가
         playerRigidbody.velocity = Vector2.zero;  // 점프 직전 속도 0,0
         
         // 몸체에 위로 힘줘서 올림, 2차원 백터기 때문에 x,y 즉 위로 힘을 주고 있는것이다
         playerRigidbody.AddForce(new Vector2(0, jumpForce));  
         
         playerAudio.Play();  // 소리 재생
      } // 2단 점프만 가능 점프 카운트의 상한을 높이면 다중 점프 가능
      // 점프 실행시 점프 소리 재생도 같이 겸하고 있음
      
      // 마우스에서 손때는 순간 이면서 속도의 Y값이 양수일때(상승중)
      else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
      {
         // 현재 속도를 절반으로 바꿈
         playerRigidbody.velocity = playerRigidbody.velocity * 0.5f; 
      }
      
      // 애니메이터의 Grounded 파라미터를 isGrounded값으로 변경
      animator.SetBool("Grounded", isGrounded);
      /*
         SetBool(또는 SetInt,SetFloat) (string, val);
         string부분엔 에니메이터에서 설정한 변수명, 뒤val에는 Set할 변수와 같은 형을 가진 변수 또는 값을 넣어주면 된다.
       */
   }

   // 사망 처리
   private void Die() {
      // 트리거 땅기기
      animator.SetTrigger("Die");
      playerAudio.clip = deathClip; // 오디오에 설정된 소스를 변수를 이용해 변경
      playerAudio.Play();  // 변경후 바로 실행
      
      playerRigidbody.velocity = Vector2.zero;  // 속도 0
      isDead = true; // 사망 상태
   }

// 트리거 콜라이더를 가진 장애물과의 충돌을 감지
   private void OnTriggerEnter2D(Collider2D other) {
      // 테그로는 죽음이긴 하지만, 죽은 상태가 아니였을때 죽음 상태로 만들기
      if (other.tag == "Dead" && !isDead)
      {
         Die();
      }
   }
   
// 바닥에 닿았음을 감지하는 처리
   private void OnCollisionEnter2D(Collision2D collision) {
      // 다른 콜리전과 충돌 했고, 그 콜리전의 표면이 위쪽을 보고 있다면
      // y가 0.7이상일때를 체크하는 이유는 기울기가 1인 직선의 법선 정규화 백터는 (-0.7,0.7)을 가짐
      // 따라서 아래의 if문의 조건은 단순히 콜리전의 윗부분에 접했는지를 확인 하는 것 뿐만 아니라 경사가 45도 이상인지 아닌지를 체크하는 부분이기도 함
      // *참고 : 아래일땐 (0,-1) 위에 있을떈 (0,1) 우측일땐(1,0) 마지막으로 좌측일땐 (-1,0) 을 가진다
      if (collision.contacts[0].normal.y > 0.7f)
      {
         // 땅에 닿은 판정과 동시에 점프 횟수를 초기화 시킴
         isGrounded = true;
         jumpCount = 0;
      }
   }
   
// 바닥에서 벗어났음을 감지하는 처리
   private void OnCollisionExit2D(Collision2D collision) {
      // 콜리전과 떨어진 상태라면 
      isGrounded = false;
   }
}