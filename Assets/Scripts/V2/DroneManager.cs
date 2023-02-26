using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private bool[] inplace;
    private bool fullPlace = false;
    [SerializeField] private int minimumOrder;
    [SerializeField] private int maximumOrder;
    [SerializeField] private Vector3 heightOffset;
    [SerializeField] private float orderSpawnDelay = 10f;
    [SerializeField] private GameObject droneDelivery;
    public bool DroneActive { get; private set; }
    private void Start()
    {
        inplace = new bool[spawnPoints.Length];
    }

    public void ActivateOrder(bool value)
    {
        DroneActive = value;
        if(DroneActive)
        {
            SpawnDelivery();
            StartCoroutine(OrderControl());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator OrderControl()
    {
        while (true)
        {
            yield return new WaitForSeconds(orderSpawnDelay);
            if(SpawnDelivery())
            {
                yield return null;
            }
            else
            {
                while (fullPlace)
                {
                    yield return null;
                }
            }
        }
    }

    private bool SpawnDelivery()
    {
        int freeSlot = RandomFreeSlot(inplace);
        if(freeSlot == -99)
        {
            return false;
        }
        GameObject go = Instantiate(droneDelivery, spawnPoints[freeSlot].position + heightOffset, Quaternion.identity);
        CookingOrder cko = go.GetComponent<CookingOrder>();
        if(cko != null)
        {
            RoboController rc = go.GetComponent<RoboController>();
            if (rc != null)
            {
                CookingOrder.OrderDetail newOrder = CookingManager.Instance.RequestOrder();
                if (cko.AssignOrder(newOrder))
                {
                    inplace[freeSlot] = true;
                    cko.droneSlot = freeSlot;
                    CookingManager.Instance.AddOrder(newOrder);
                    rc.Flying(RoboController.FlyDirection.Down, spawnPoints[freeSlot].position);
                }
                else
                {
                    Destroy(go);
                    return false;
                }
            }
            else
            {
                Destroy(go);
                return false;
            }
        }
        else
        {
            Destroy(go);
            return false;
        }
        return true;
    }

    private int RandomFreeSlot(bool[] array)
    {
        int random = Random.Range(0, array.Length);
        if (array[random] == false)
        {
            return random;
        }
        else
        {
            bool fullSlot = true;
            foreach (var slot in array)
            {
                if(slot == false)
                {
                    fullSlot = false;
                    break;
                }
            }
            if (fullSlot == false)
            {
                return RandomFreeSlot(array);
            }
            else
            {
                fullPlace = true;
                return -99;
            }
        }
    }

    public void FreeSlot(int slot)
    {
        if(slot < 0)
        {
            return;
        }
        inplace[slot] = false;
        if(fullPlace)
        {
            fullPlace = false;
        }
    }
}
