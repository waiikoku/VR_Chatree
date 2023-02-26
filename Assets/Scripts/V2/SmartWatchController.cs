using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SmartWatchController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    public int panelIndex = 0;
    [SerializeField] private bool panelLoop = false;
    [SerializeField] private GameObject[] panels;
    private void Start()
    {
        GameManager.Instance.OnMoneyChange += UpdateMoney;
        GameManager.Instance.UpdateMoney();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoneyChange -= UpdateMoney;
    }

    private void UpdateMoney(int value)
    {
        moneyText.text = value.ToString();
    }

    public void NextPanel()
    {
        panels[panelIndex].SetActive(false);
        panelIndex++;
        if (panelLoop)
        {
            if (panelIndex >= panels.Length - 1) panelIndex = 0;
        }
        panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
        panels[panelIndex].SetActive(true);
    }

    public void BackPanel()
    {
        panels[panelIndex].SetActive(false);
        panelIndex--;
        if (panelLoop)
        {
            if (panelIndex <= 0) panelIndex = panels.Length - 1;
        }
        panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length);
        panels[panelIndex].SetActive(true);
    }
}
