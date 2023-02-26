using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoboController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool toggle;
    [SerializeField] private float flyingDuration = 5f;
    [SerializeField] private TextMeshProUGUI displayStatus;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.XRSocketInteractor placeSocket;
    [SerializeField] private CookingOrder roboOrder;
    public void ToggleLocker()
    {
        toggle = !toggle;
        if(toggle)
        {
            anim.SetBool("Open",true);
            displayStatus.text = "Click to Close";
        }
        else
        {
            SetHinge(0);
            displayStatus.text = "Click to Open";
        }
    }

    public void Turnoff()
    {
        SetHinge(0);
        displayStatus.text = "Click to Open";
    }

    public void SetHinge(int boolean)
    {
        bool result = boolean == 1 ?true :false;
        anim.SetBool("Hinge", result);
    }

    public void SetLocker(int boolean)
    {
        bool result = boolean == 1 ? true : false;
        anim.SetBool("Open", result);
    }
    public enum FlyDirection
    {
        Invalid,
        Stay,
        Up,
        Down
    }
    public void Flying(FlyDirection direction,Vector3 destination)
    {
        switch (direction)
        {
            case FlyDirection.Invalid:
                break;
            case FlyDirection.Stay:
                break;
            case FlyDirection.Up:
                StartCoroutine(FlyingThread(destination,delegate { Destroy(gameObject); }));
                break;
            case FlyDirection.Down:
                StartCoroutine(FlyingThread(destination,delegate { roboOrder.BeginOrder(); }));
                break;
            default:
                break;
        }
    }

    private Vector3 sdVelocity;
    private IEnumerator FlyingThread(Vector3 destination,Action callback = null)
    {
        while (true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination,ref sdVelocity,flyingDuration);
            if(Vector3.Distance(transform.position,destination) < 0.1f)
            {
                break;
            }
            yield return null;
        }
        callback?.Invoke();
    }
}
