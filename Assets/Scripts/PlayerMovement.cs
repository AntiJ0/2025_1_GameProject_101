using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;
    //이동 속도 변수 설정
    public float jumpForce = 7f;
    //점프의 힘 값 설정
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;
    //땅에 있는지 체크하는 변수(true/false)

    public int coinCount = 0;
    //코인 획득 변수 선언
    public int totalCoins = 5;
    //총 코인 획득 필요 변수 선언

    public Rigidbody rb;
    //플레이어의 강체 선언

    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;

        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdateGroundedState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동 방향 벡터
        Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

        //입력이 있을 때만 회전
        if(movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //G키로 글라이더 제어(누르는 동안만 활성화)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //글라이더 활성화 함수
                EnableGlider();
            }

            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if(gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            //G키를 떼면 글라이더 비활성화
            DisableGlider();
        }

        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            //속도로 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                //하강 시 중력 강화
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)
        //&& 두 값을 만족할 때 -> 스페이스 버튼을 눌렸을때 외 isGrounded가 True 일때
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //위쪽으로 설정한 값만큼의 힘을 강체에 준다
            isGrounded = false;
            //점프를 하는 순간 땅에서 떨어졌기 때문에 false로 설정
            realGrounded = false;
            coyoteTimeCounter = 0;
        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }
    }

    //글라이더 활성화 함수
    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르게 이동

        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
            );

        rb.velocity = gliderVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = true;
            //땅과 충돌하면 true로 변경한다
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //코인 수집
        if(other.CompareTag("Coin"))
        //코인 트리거와 충돌하면
        {
            coinCount++;
            //코인 변수를 1 올려줌
            Destroy(other.gameObject);
            //코인 오브젝트를 없앰
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        //목적지 도착 시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount >= totalCoins)
        //모든 코인을 획득후에 문으로 가면 게임 종료
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 가능
        }
    }

    void UpdateGroundedState()
    {
        if (realGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
