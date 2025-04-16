using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;
    //�̵� �ӵ� ���� ����
    public float jumpForce = 7f;
    //������ �� �� ����
    public float turnSpeed = 10f;

    [Header("���� ���� ����")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;
    //���� �ִ��� üũ�ϴ� ����(true/false)

    public int coinCount = 0;
    //���� ȹ�� ���� ����
    public int totalCoins = 5;
    //�� ���� ȹ�� �ʿ� ���� ����

    public Rigidbody rb;
    //�÷��̾��� ��ü ����

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
        //���� ���� ����ȭ
        UpdateGroundedState();

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

        //�Է��� ���� ���� ȸ��
        if(movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //GŰ�� �۶��̴� ����(������ ���ȸ� Ȱ��ȭ)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //�۶��̴� Ȱ��ȭ �Լ�
                EnableGlider();
            }

            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;

            //�۶��̴� �ð��� �� �Ǹ� ��Ȱ��ȭ
            if(gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ�
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            //GŰ�� ���� �۶��̴� ��Ȱ��ȭ
            DisableGlider();
        }

        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            //�ӵ��� ���� �̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //���� ���� ���� ����
            if (rb.velocity.y < 0)
            {
                //�ϰ� �� �߷� ��ȭ
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)
        //&& �� ���� ������ �� -> �����̽� ��ư�� �������� �� isGrounded�� True �϶�
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //�������� ������ ����ŭ�� ���� ��ü�� �ش�
            isGrounded = false;
            //������ �ϴ� ���� ������ �������� ������ false�� ����
            realGrounded = false;
            coyoteTimeCounter = 0;
        }

        //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }
    }

    //�۶��̴� Ȱ��ȭ �Լ�
    void EnableGlider()
    {
        isGliding = true;

        //�۶��̴� ������Ʈ ǥ��
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

    //�۶��̴� �̵� ����
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //�۶��̴� ȿ�� : õõ�� �������� ���� �������� �� ������ �̵�

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
            //���� �浹�ϸ� true�� �����Ѵ�
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
        //���� ����
        if(other.CompareTag("Coin"))
        //���� Ʈ���ſ� �浹�ϸ�
        {
            coinCount++;
            //���� ������ 1 �÷���
            Destroy(other.gameObject);
            //���� ������Ʈ�� ����
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        //������ ���� �� ���� �α� ���
        if(other.gameObject.tag == "Door" && coinCount >= totalCoins)
        //��� ������ ȹ���Ŀ� ������ ���� ���� ����
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
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
