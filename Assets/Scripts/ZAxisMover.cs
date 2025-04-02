using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//5초 동안 5의 스피드로 앞으로 이동하고 사라지는 컴포넌트 클래스
public class ZAxisMover : MonoBehaviour
//z축으로 이동하는 클래스
{
    public float speed = 5.0f;
    public float timer = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer < 0 )
        {
            Destroy(gameObject);
        }
    }
}
