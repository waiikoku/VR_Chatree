using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public int money;
    public bool IsStart { get; private set; }
    public event Action<int> OnMoneyChange;

    [SerializeField] private CookingManager cookingManager;
    [SerializeField] private DroneManager droneManager;

    [SerializeField] private int porkCost;
    public int PorkPrice => porkCost;
    [SerializeField] private int[] engineCost;

    private float playtime;
    private int cookedMeat;
    private int burnMeat;
    private int completeOrder;
    private int failedOrder;
    private int gainMoney;
    private int spentMoney;

    [Header("UI")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI timerT;
    [SerializeField] private TextMeshProUGUI coinT;
    [SerializeField] private TextMeshProUGUI comOrdT;
    [SerializeField] private TextMeshProUGUI failOrdT;
    [SerializeField] private TextMeshProUGUI gainMT;
    [SerializeField] private TextMeshProUGUI sptMT;

    [Header("Debug")]
    [SerializeField] private bool debugMode =false;
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private float cheat_Money = 500f;
    [SerializeField] private TextMeshProUGUI cheatMoneyLabel;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        cookingManager.OnSubmit += OrderStatistic;
        cheatPanel.SetActive(debugMode);
        cheatMoneyLabel.text = cheat_Money.ToString();
    }

    private void LateUpdate()
    {
        if(IsStart)
        {
            playtime += Time.deltaTime;
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        gainMoney += amount;
        OnMoneyChange?.Invoke(money);
        SoundManager.Instance.Purchase();
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        spentMoney += amount;
        OnMoneyChange?.Invoke(money);
        SoundManager.Instance.Purchase();
    }

    private void OrderStatistic(CookingOrder.OrderDetail detail)
    {
        if(detail.completion == CookingOrder.OrderDetail.Completion.Completed)
        {
            completeOrder++;
        }else if(detail.completion == CookingOrder.OrderDetail.Completion.Failed)
        {
            failedOrder++;
        }
    }

    private void DisplayResult()
    {
        timerT.text = $"Playtime: {Mathf.RoundToInt(playtime)}s";
        coinT.text = $"Coin: {money}";
        comOrdT.text = $"Complete: {completeOrder} Order";
        failOrdT.text = $"Failed: {failedOrder} Order";
        gainMT.text = $"Gain: {gainMoney} Coin";
        sptMT.text = $"Spent: {spentMoney} Coin";
    }

    public bool BuyMeat()
    {
        if (money - porkCost < 0)
        {
            return false;
        }
        RemoveMoney(porkCost);
        return true;
    }

    public bool BuyEngine(int index = 0)
    {
        if (money - engineCost[index] < 0)
        {
            return false;
        }
        RemoveMoney(engineCost[index]);
        return true;
    }

    public void UpdateMoney()
    {
        OnMoneyChange?.Invoke(money);
    }

    public void StartGame()
    {
        IsStart = true;
        droneManager.ActivateOrder(true);
    }

    public void EndGame(bool isFailed = false)
    {
        IsStart = false;
        droneManager.ActivateOrder(false);
        if (isFailed == false)
        {
            resultPanel.SetActive(true);
            DisplayResult();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
