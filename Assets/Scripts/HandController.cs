using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    ActionBasedController controller;
    public HandGrip hand;
    public Vector2 axis;
    public Vector2 axis2;
    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
        /*
        controller.selectAction.action.performed += delegate { hand.SetGrip(1f); };
        controller.selectAction.action.canceled += delegate { hand.SetGrip(0f); };
        */
    }

    private void Update()
    {
        hand.SetGrip(controller.selectActionValue.action.ReadValue<float>());
        hand.SetTrigger(controller.activateActionValue.action.ReadValue<float>());
    }

    private void OnGrip(InputAction.CallbackContext callback)
    {

    }
}
