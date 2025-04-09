using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        //z축 -방향으로 이동
        transform.Translate(0, 0, -moveSpeed * Time.deltaTime); 

        if(transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
}
