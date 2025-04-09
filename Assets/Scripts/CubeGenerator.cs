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
            //�� ��ġ���� z������ cubeSpacing * i��ŭ ������ ��ġ�� ����
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            //ť�� ����
            Instantiate(cubePrefab, position, Quaternion.identity); 
        }
    }
}
