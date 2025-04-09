using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCubes = 10;
    public float cubeSpacing = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position;

        GameObject firstCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);

        for (int i = 0; i < totalCubes; i++)
        {
            //내 위치에서 z축으로 cubeSpacing * i만큼 떨어진 위치에 생성
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            //큐브 생성
            Instantiate(cubePrefab, position, Quaternion.identity); 
        }
    }
}
