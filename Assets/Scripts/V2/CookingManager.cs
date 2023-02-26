using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CookingOrder;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Instance { get; private set; }

    public List<OrderDetail> orderCompletion;
    [SerializeField] private DroneManager droneManager;

    [SerializeField] private int minimumReward;
    [SerializeField] private int maximumReward;

    [SerializeField] private int minimumQueueDuration;
    [SerializeField] private int maximumQueueDuration;

    [SerializeField] private int minimumWaitDuration;
    [SerializeField] private int maximumWaitDuration;

    public event System.Action<OrderDetail> OnSubmit;
    private void Awake()
    {
       if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        orderCompletion = new List<OrderDetail>();
    }

    public OrderDetail RequestOrder()
    {
        OrderDetail order = new OrderDetail();
        order.orderID = orderCompletion.Count;
        order.requireMeat = Random.Range(0, 100) > 90 ? CookState.Burn : CookState.Cooked;
        order.orderReward = Random.Range(minimumReward, maximumReward);
        order.queueDuration = Random.Range(minimumQueueDuration, maximumQueueDuration);
        order.orderDuration = Random.Range(minimumWaitDuration, maximumWaitDuration);
        order.completion = OrderDetail.Completion.Doing;
        return order;
    }

    public void AddOrder(OrderDetail detail)
    {
        orderCompletion.Add(detail);
    }

    public void UpdateOrder(OrderDetail order,CookingOrder cko = null)
    {
        var tempOrder = orderCompletion[order.orderID];
        tempOrder.completion = order.completion;
        orderCompletion[order.orderID] = tempOrder;
        OnSubmit?.Invoke(order);
        if(tempOrder.completion == OrderDetail.Completion.Completed)
        {
            GameManager.Instance.AddMoney(tempOrder.orderReward);
        }
        if(cko != null)
        {
            droneManager.FreeSlot(cko.droneSlot);
        }
    }
}
