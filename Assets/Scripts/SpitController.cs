using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class SpitController : MonoBehaviour
{
    XRBaseInteractable m_Interactable;

    [SerializeField] private Collider meat;
    protected void OnEnable()
    {
        m_Interactable = GetComponent<XRBaseInteractable>();

        m_Interactable.selectEntered.AddListener(OnSelectEntered);
        m_Interactable.selectExited.AddListener(OnSelectExited);
    }

    protected void OnDisable()
    {
        m_Interactable.selectEntered.RemoveListener(OnSelectEntered);
        m_Interactable.selectExited.RemoveListener(OnSelectExited);
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        meat.isTrigger = true;
    }
    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {

        meat.isTrigger = false;
    }
}
