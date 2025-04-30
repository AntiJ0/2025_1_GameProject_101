using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public int cardValue;
    public Sprite cardImage;
    public TextMeshPro cardText;

    //카드 정보 초기화 함수
    public void InitCard(int value, Sprite image)
    {
        cardValue = value;
        cardImage = image;

        GetComponent<SpriteRenderer>().sprite = image;

        if (cardText != null)
        {
            cardText.text = cardValue.ToString();
        }
    }
}
