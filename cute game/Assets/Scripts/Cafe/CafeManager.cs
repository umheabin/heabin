using System;
using UnityEngine;

public class CafeManager : MonoBehaviour
{
    public int level = 1;
    public int money = 0;
    public int coffeeSold = 0;
    public int breadSold = 0;
    public int parfaitSold = 0;
    public int cookiesSold = 0;

    public bool canSellCoffee = false;
    public bool canSellBread = false;
    public bool canSellParfait = false;
    public bool canSellCookies = false;

    public GameObject coffeeObject;
    public GameObject breadObject;
    public GameObject parfaitObject;
    public GameObject cookiesObject;
    public GameObject rabbitCustomerObject;
    public Transform playerPosition;  // 주인공 곰돌이 위치
    public Transform rabbitStartPosition;  // 토끼 손님 시작 위치
    public float distanceToOrder = 2f;  // 주문하기 전에 토끼와 주인공 사이의 거리 (예: 2 유닛 내)
    public float rotationSpeed = 5f;  // 회전 속도

    private string currentOrder = "";
    private bool isRabbitAtCounter = false;

    void Start()
    {
        UpdateFoodAvailability();
        HideFoodObjects();
        InvokeRepeating("RabbitCustomerArrives", 5f, 10f); // 토끼 손님 10초마다 등장

        // 주인공 위치가 할당되지 않으면 게임 시작 시 초기화가 필요
        if (playerPosition == null)
        {
            playerPosition = GameObject.FindWithTag("Player").transform;  // 주인공이 Player 태그로 지정된 경우
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && currentOrder == "coffee")
        {
            SellCoffee();
        }
        if (Input.GetKeyDown(KeyCode.B) && currentOrder == "bread")
        {
            SellBread();
        }
        if (Input.GetKeyDown(KeyCode.P) && currentOrder == "parfait")
        {
            SellParfait();
        }
        if (Input.GetKeyDown(KeyCode.K) && currentOrder == "cookies")
        {
            SellCookies();
        }

        CheckLevelUp();
    }

    void RabbitCustomerArrives()
    {
        string[] possibleOrders = GetPossibleOrders();
        if (possibleOrders.Length > 0)
        {
            currentOrder = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Length)];
            Debug.Log("🐇 토끼 손님이 " + currentOrder + "를 주문했습니다!");
            MoveRabbitToPlayer();  // 주인공에게 다가가서 주문하기
        }
        else
        {
            Debug.Log("🐇 주문할 음식이 없어서 토끼가 그냥 떠났습니다.");
        }
    }

    void MoveRabbitToPlayer()
    {
        // 주인공에게 다가가기
        StartCoroutine(MoveToPosition(rabbitStartPosition.position, playerPosition.position, 3f));  // 3초 동안 이동
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;

        // Y값을 고정해서 이동하도록 설정
        float startY = startPosition.y;
        float endY = endPosition.y;

        while (elapsedTime < duration)
        {
            float newX = Mathf.Lerp(startPosition.x, endPosition.x, elapsedTime / duration);
            float newZ = Mathf.Lerp(startPosition.z, endPosition.z, elapsedTime / duration);
            // Y값 고정
            float newY = startY;

            rabbitCustomerObject.transform.position = new Vector3(newX, newY, newZ);  // X, Y, Z 값을 모두 계산하여 이동

            // 토끼가 주인공을 바라보게 하기 (y 회전만 적용)
            Vector3 direction = playerPosition.position - rabbitCustomerObject.transform.position;
            direction.y = 0;  // Y 축 회전은 제외하고 평면에서만 회전하도록 설정
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rabbitCustomerObject.transform.rotation = Quaternion.Slerp(rabbitCustomerObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rabbitCustomerObject.transform.position = new Vector3(endPosition.x, startY, endPosition.z);  // 주인공에게 도달하면 최종 위치로 설정

        // 주문을 받기 전에 일정 거리 이내에 도달하면 주문하기
        if (Vector3.Distance(rabbitCustomerObject.transform.position, playerPosition.position) < distanceToOrder)
        {
            PlaceOrder();
        }
    }

    void PlaceOrder()
    {
        Debug.Log("🐇 토끼가 주문한 음식: " + currentOrder);
        // 주문에 따른 말 하기
        switch (currentOrder)
        {
            case "coffee":
                Debug.Log("🐇 커피를 주문하셨어요!");
                break;
            case "bread":
                Debug.Log("🐇 빵을 주문하셨어요!");
                break;
            case "parfait":
                Debug.Log("🐇 파르페를 주문하셨어요!");
                break;
            case "cookies":
                Debug.Log("🐇 쿠키를 주문하셨어요!");
                break;
            default:
                Debug.Log("🐇 주문할 음식이 없어요!");
                break;
        }

        currentOrder = "";  // 주문 완료 후 초기화
    }

    string[] GetPossibleOrders()
    {
        var orders = new System.Collections.Generic.List<string>();
        if (canSellCoffee) orders.Add("coffee");
        if (canSellBread) orders.Add("bread");
        if (canSellParfait) orders.Add("parfait");
        if (canSellCookies) orders.Add("cookies");
        return orders.ToArray();
    }

    void SellCoffee()
    {
        if (currentOrder == "coffee")
        {
            money += 5;
            coffeeSold++;
            ShowFoodObject(coffeeObject);
            Debug.Log("☕ 커피 판매! 현재 돈: " + money);
            currentOrder = "";
        }
    }

    void SellBread()
    {
        if (currentOrder == "bread")
        {
            money += 10;
            breadSold++;
            ShowFoodObject(breadObject);
            Debug.Log("🍞 빵 판매! 현재 돈: " + money);
            currentOrder = "";
        }
    }

    void SellParfait()
    {
        if (currentOrder == "parfait")
        {
            money += 15;
            parfaitSold++;
            ShowFoodObject(parfaitObject);
            Debug.Log("🍨 파르페 판매! 현재 돈: " + money);
            currentOrder = "";
        }
    }

    void SellCookies()
    {
        if (currentOrder == "cookies")
        {
            money += 8;
            cookiesSold++;
            ShowFoodObject(cookiesObject);
            Debug.Log("🍪 쿠키 판매! 현재 돈: " + money);
            currentOrder = "";
        }
    }

    void CheckLevelUp()
    {
        if (coffeeSold >= 10) { level++; coffeeSold = 0; UpdateFoodAvailability(); }
        if (breadSold >= 5) { level++; breadSold = 0; UpdateFoodAvailability(); }
        if (parfaitSold >= 3) { level++; parfaitSold = 0; UpdateFoodAvailability(); }
        if (cookiesSold >= 4) { level++; cookiesSold = 0; UpdateFoodAvailability(); }
    }

    void UpdateFoodAvailability()
    {
        canSellCoffee = level >= 1;
        canSellBread = level >= 16;
        canSellParfait = level >= 19;
        canSellCookies = level >= 24;
    }

    void ShowFoodObject(GameObject foodObject)
    {
        foodObject.SetActive(true);
        Invoke("HideFoodObjects", 2f);
    }

    void HideFoodObjects()
    {
        coffeeObject.SetActive(false);
        breadObject.SetActive(false);
        parfaitObject.SetActive(false);
        cookiesObject.SetActive(false);
    }
}
