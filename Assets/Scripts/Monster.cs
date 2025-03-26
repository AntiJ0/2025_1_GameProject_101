using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100;
    //ü��(Health) ����
    public float Timer = 1.0f;
    //Ÿ�̸�(Timer) ����
    public int AttackPoint = 10;
    //���ݷ�(AttackPoint) ����

    //ù ������ ������ �� �� ����
    void Start()
    {
        Health = 100;
        //ù ������ ������ ����� �� 100 ü��(Health)�� �߰�
    }

    //�� �����Ӹ��� ȣ��
    void Update()
    {
        CharacterHealthUp();
        CheckDeath();
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime;
        //�ð��� �� �����Ӹ��� ���ҽ�Ų��

        if (Timer <= 0)
        //���� Timer�� ��ġ�� 0 ���Ϸ� ������ ���
        {
            Timer = 1.0f;
            //Timer�� 1.0���� ����
            Health += 20;
            //Health�� 20��ŭ ���
        }
    }

    public void CharacterHit(int Damage)
    //Ŀ���� �������� �޴� �Լ� ���
    {
        Health -= Damage;
    }

    void CheckDeath()
    //�Լ� ����
    {
        if (Health <= 0)
        //ü���� 0 ���϶��
            Destroy(gameObject);
        //gameObject�� �ı�
    }
}
