using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public float invincibleTime = 1.0f;
    public bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            currentLives--;
            Destroy(other.gameObject);

            if (currentLives <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        //플레이어 비활성화
        gameObject.SetActive(false);
        //플레이어가 안보인다
        Invoke("RestartGame", 3.0f);
        //3초후 RestartGame 함수 호출
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
