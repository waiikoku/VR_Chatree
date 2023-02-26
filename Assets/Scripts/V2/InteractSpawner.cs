using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMPro.TextMeshProUGUI meatPrice;

    private void Start()
    {
        meatPrice.text = $"-{GameManager.Instance.PorkPrice}";
    }

    public void Spawn()
    {
        if (GameManager.Instance.BuyMeat())
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {

        }

    }
}
