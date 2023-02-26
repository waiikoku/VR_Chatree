using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Transform m_transform;
    private enum MenuState
    {
        Main,
        Gameplay
    }
    [SerializeField] private MenuState state;
    [SerializeField] private Button b_engine1;
    [SerializeField] private Button b_engine2;
    [SerializeField] private Button b_engine3;
    [SerializeField] private GameObject[] bannerEngines;
    [SerializeField] private GameObject[] boughtEngines;
    [SerializeField] private Button startEngine;
    [SerializeField] private SpaceshipController ssc;
    [SerializeField] private AsteroidManager asteroidManager;
    private void Start()
    {
        m_transform = transform;
        b_engine1.onClick.AddListener(delegate { BuyEngine(0); });
        b_engine2.onClick.AddListener(delegate { BuyEngine(1); });
        b_engine3.onClick.AddListener(delegate { BuyEngine(2); });
        startEngine.onClick.AddListener(delegate { GameManager.Instance.EndGame(); ssc.TakeOff(); asteroidManager.StopMeteor(); });
        ssc.OnEngineFixed += ReactiveEngine;
    }

    private void OnDestroy()
    {
        ssc.OnEngineFixed -= ReactiveEngine;
    }

    public void ChangeToGameplay()
    {
        state = MenuState.Gameplay;
        m_transform.localRotation = Quaternion.Euler(0, -180, 0);
    }

    public void ChangeToMain()
    {
        state = MenuState.Main;
        m_transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void BuyEngine(int index)
    {
        if(GameManager.Instance.BuyEngine(index))
        {
            bannerEngines[index].SetActive(false);
            ssc.FixEngine(index, true);
            boughtEngines[index].SetActive(true);
        }
    }

    private void ReactiveEngine()
    {
        startEngine.interactable = true;
    }
}
