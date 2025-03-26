using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    //이동 속도 변수 설정
    public float jumpForce = 5f;
    //점프의 힘 값 설정

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
        
    }

    // Update is called once per frame
    void Update()
    {
        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)
        //&& 두 값을 만족할 때 -> 스페이스 버튼을 눌렸을때 외 isGrounded가 True 일때
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //위쪽으로 설정한 값만큼의 힘을 강체에 준다
            isGrounded = false;
            //점프를 하는 순간 땅에서 떨어졌기 때문에 false로 설정
        }        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            //땅과 충돌하면 true로 변경한다
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
}
