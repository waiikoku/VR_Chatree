using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CookingOrder : MonoBehaviour
{
    public enum CookState
    {
        Null,
        Rare,
        Cooked,
        Burn
    }

    public CookState RequireOrder = CookState.Null;
    public int rewardMoney = 0;
    public bool onQueue = false;
    public bool acceptedOrder = false;
    public float waitingQueueDuration = 1f;
    public float waitingOrderDuration = 1f;
    private float timerQueue;
    private float timerOrder;
    private float queuePercentage;
    private float orderPercentage;

    public CookState currentMeat;
    private MeatController currentCook;
    public int orderNumber = 0;
    public int droneSlot = -1;
    public bool hasLanded = false;
    public bool orderCompleted = false;
    private bool hasSubmit = false;
    private OrderDetail currentOrderDetail;
    [SerializeField] private RoboController robo;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private TextMeshProUGUI rewardText;
    //Object Requirement Static
    [SerializeField] private GameObject rawMeat;
    [SerializeField] private GameObject cookMeat;
    [SerializeField] private GameObject burnMeat;
    [SerializeField] private Image cookImage;
    [SerializeField] private Image timerImage;
    [SerializeField] private Button btn_AcceptOrder;
    [SerializeField] private Button btn_CancelOrder;
    [SerializeField] private Button btn_SubmitOrder;

    private void Start()
    {
        btn_AcceptOrder.onClick.AddListener(AcceptOrder);
        btn_CancelOrder.onClick.AddListener(CancelOrder);
        btn_SubmitOrder.onClick.AddListener(delegate { robo.Flying(RoboController.FlyDirection.Up, transform.position + (Vector3.up * 100)); SubmitOrder(); });
    }

    private void LateUpdate()
    {
        if (onQueue == false) return;
        if (acceptedOrder == false)
        {
            timerQueue += Time.deltaTime;
            queuePercentage = timerQueue / waitingQueueDuration;
            timerImage.fillAmount = queuePercentage;
            timerText.text = $"{timerQueue:F0}s / {waitingQueueDuration}s";
            if (timerQueue > waitingQueueDuration)
            {
                onQueue = false;
                robo.Flying(RoboController.FlyDirection.Up, transform.position + (Vector3.up * 100));
                SubmitOrder();
            }
        }
        else
        {
            timerOrder += Time.deltaTime;
            orderPercentage = timerOrder / waitingOrderDuration;
            timerImage.fillAmount = orderPercentage;
            timerText.text = $"{timerOrder:F0}s / {waitingOrderDuration}s";
            if (timerOrder > waitingOrderDuration)
            {
                robo.Flying(RoboController.FlyDirection.Up, transform.position + (Vector3.up * 100));
                //Penealty
                SubmitOrder(true);
            }
        }
        if(currentCook != null)
        {
            cookImage.fillAmount = currentCook.cookingProgress / 100f;
        }
    }

    public void PlaceMeat(MeatController meat)
    {
        currentCook = meat;
        float minC = meat.minCook * 100f;
        float maxC = meat.maxCook * 100f;
        if (meat.cookingProgress <= minC)
        {
            currentMeat = CookState.Rare;
        }
        else if(meat.cookingProgress <= maxC)
        {
            currentMeat = CookState.Cooked;
        }
        else if(meat.cookingProgress >= maxC)
        {
            currentMeat = CookState.Burn;
        }
    }

    public bool RemoveMeat()
    {
        if (hasSubmit)
        {
            return false;
        };
        currentMeat = CookState.Null;
        currentCook = null;
        cookImage.fillAmount = 0f;
        return true;
    }

    public bool AssignOrder(OrderDetail detail)
    {
        try
        {
            orderNumber = detail.orderID;
            orderText.text = $"Order#{detail.orderID}";
            rewardMoney = detail.orderReward;
            rewardText.text = rewardMoney.ToString();
            waitingQueueDuration = detail.queueDuration;
            waitingOrderDuration = detail.orderDuration;
            RequireOrder = detail.requireMeat;
            switch (RequireOrder)
            {
                case CookState.Rare:
                    rawMeat.SetActive(true);
                    break;
                case CookState.Cooked:
                    cookMeat.SetActive(true);
                    break;
                case CookState.Burn:
                    burnMeat.SetActive(true);
                    break;
                default:
                    break;
            }
            currentOrderDetail = detail;
            timerImage.fillAmount = queuePercentage;
            timerText.text = $"{timerQueue:F0}s / {waitingQueueDuration}s";
        }
        catch (System.Exception e)
        {
            print(e.Message);
            return false;
            throw;
        }
        return true;
    }

    public void BeginOrder()
    {
        onQueue = true;
    }

    public void CancelOrder()
    {
        robo.Flying(RoboController.FlyDirection.Up, transform.position + (Vector3.up * 100));
        SubmitOrder();
    }

    public void AcceptOrder()
    {
        acceptedOrder = true;
    }

    /// <summary>
    /// Submit Placed Order OR Cancel Order by sending Null
    /// </summary>
    public void SubmitOrder(bool penealty = false)
    {
        hasSubmit = true;
        robo.Turnoff();
        if(currentCook != null)
        {
            Destroy(currentCook.gameObject,2f);
        }
        if(RequireOrder == currentMeat)
        {
            currentOrderDetail.completion = OrderDetail.Completion.Completed;
        }
        else
        {
            currentOrderDetail.completion = OrderDetail.Completion.Failed;
        }
        CookingManager.Instance.UpdateOrder(currentOrderDetail,this);
    }

    public struct OrderDetail
    {
        public int orderID;
        public int orderReward;
        public float queueDuration;
        public float orderDuration;
        public CookState requireMeat;
        public enum Completion
        {
            Invalid,
            Doing,
            Completed,
            Failed
        }
        public Completion completion;
    }

}
