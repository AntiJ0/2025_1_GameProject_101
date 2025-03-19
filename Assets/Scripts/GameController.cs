using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 3.0f;
    //게임 타이머 설정
    public GameObject MonsterGO;

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;

        if (GameTimer <= 0)
        {
            GameTimer = 3.0f;

            GameObject Temp = Instantiate(MonsterGO);
            //해당 게임 오브젝트를 복사 생성
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            //x -10 ~ 10, y -4 ~ 4 의 범위에 랜덤으로 위치 시킴
        }

        if (Input.GetMouseButtonDown(0))
        //마우스 버튼이 눌렸을 때
        {
            RaycastHit hit;
            //Ray 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //카메라에서 Ray를 쏴서 검충
            //주로 3D 게임에서 오브젝트를 검출(화면에 보이는 물체를 선택)할 때 사용

            if (Physics.Raycast(ray, out hit))
            //Ray에 hit 된 오브젝트 검출
            {
                if (hit.collider != null)
                //hit 된 오브젝트가 있다면
                {
                    Debug.Log(hit.collider.name);
                    //로그로 표시 
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);
                    //몬스터에게 데미지 50을 가함
                }
            }
        }
    }
}
