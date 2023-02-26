using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeatSocket : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor interactor;
    [SerializeField] private Transform container;
    [SerializeField] private CookingOrder order;
    private void Start()
    {
        interactor.selectEntered.AddListener(OnSnapped);
        interactor.selectExited.AddListener(OnUnsnap);
    }

    private void OnSnapped(SelectEnterEventArgs args)
    {
        MeatController mc = args.interactableObject.transform.GetComponent<MeatController>();
        if (mc != null)
        {
            args.interactableObject.transform.parent = container;
            print($"This meat has Cook({Mathf.RoundToInt(mc.cookingProgress)}%)");
            order.PlaceMeat(mc);
        }
    }

    private void OnUnsnap(SelectExitEventArgs args)
    {
        MeatController mc = args.interactableObject.transform.GetComponent<MeatController>();
        if (mc != null)
        {
            if (order.RemoveMeat())
            {
                print("Remove meat");
                args.interactableObject.transform.parent = null;
            }
        }
    }
}
