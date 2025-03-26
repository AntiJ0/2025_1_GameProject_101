using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100;
    //체력(Health) 선언
    public float Timer = 1.0f;
    //타이머(Timer) 선언
    public int AttackPoint = 10;
    //공격력(AttackPoint) 선언

    //첫 프레임 이전에 한 번 실행
    void Start()
    {
        Health = 100;
        //첫 프레임 이전에 실행될 때 100 체력(Health)을 추가
    }

    //매 프레임마다 호출
    void Update()
    {
        CharacterHealthUp();
        CheckDeath();
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime;
        //시간을 매 프레임마다 감소시킨다

        if (Timer <= 0)
        //만약 Timer의 수치가 0 이하로 내려갈 경우
        {
            Timer = 1.0f;
            //Timer를 1.0으로 설정
            Health += 20;
            //Health를 20만큼 상승
        }
    }

    public void CharacterHit(int Damage)
    //커스텀 데미지를 받는 함수 사용
    {
        Health -= Damage;
    }

    void CheckDeath()
    //함수 선언
    {
        if (Health <= 0)
        //체력이 0 이하라면
            Destroy(gameObject);
        //gameObject를 파괴
    }
}
