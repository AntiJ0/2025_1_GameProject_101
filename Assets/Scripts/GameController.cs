using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 3.0f;
    //���� Ÿ�̸� ����
    public GameObject MonsterGO;

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;

        if (GameTimer <= 0)
        {
            GameTimer = 3.0f;

            GameObject Temp = Instantiate(MonsterGO);
            //�ش� ���� ������Ʈ�� ���� ����
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            //x -10 ~ 10, y -4 ~ 4 �� ������ �������� ��ġ ��Ŵ
        }

        if (Input.GetMouseButtonDown(0))
        //���콺 ��ư�� ������ ��
        {
            RaycastHit hit;
            //Ray ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //ī�޶󿡼� Ray�� ���� ����
            //�ַ� 3D ���ӿ��� ������Ʈ�� ����(ȭ�鿡 ���̴� ��ü�� ����)�� �� ���

            if (Physics.Raycast(ray, out hit))
            //Ray�� hit �� ������Ʈ ����
            {
                if (hit.collider != null)
                //hit �� ������Ʈ�� �ִٸ�
                {
                    Debug.Log(hit.collider.name);
                    //�α׷� ǥ�� 
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);
                    //���Ϳ��� ������ 50�� ����
                }
            }
        }
    }
}
