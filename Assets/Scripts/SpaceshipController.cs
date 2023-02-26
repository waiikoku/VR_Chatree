using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private bool alwayUpdate = false;

    [SerializeField] private SkinnedMeshRenderer[] legs;

    [SerializeField] private SkinnedMeshRenderer ramp;

    [Range(0f, 100f)]
    [SerializeField] private float legKey;

    [Range(0f, 100f)]
    [SerializeField] private float rampKey;

    [SerializeField] private Transform door;
    [SerializeField] private Vector3 baseDoor;
    [Range(0f, 1f)]
    [SerializeField] private float doorOpen;

    [SerializeField] private float floorMinimum = 1f;
    [SerializeField] private Vector3 flyDirection;
    [SerializeField] private float flySpeed = 1f;
    private bool startFly;

    [SerializeField] private bool engine1 = false;
    [SerializeField] private bool engine2 = false;
    [SerializeField] private bool engine3 = false;
    [SerializeField] private bool broken = true;
    public event Action OnEngineFixed;
    [SerializeField] private Transform landingZone;
    [SerializeField] private Transform skyZone;
    [SerializeField] private float flyingDuration = 10f;
    private void Update()
    {
        if (alwayUpdate == false) return;
        if (legs.Length == 0 || ramp == null) return;
        for (int i = 0; i < legs.Length; i++)
        {
            legs[i].SetBlendShapeWeight(0, legKey);
        }
        ramp.SetBlendShapeWeight(0, rampKey);
        if (door == null) return;
        door.localRotation = Quaternion.Euler(baseDoor + new Vector3(0, 0, -90f * doorOpen));
    }

    public void Landing()
    {
        sdVelocity = Vector3.zero;
        Flying(RoboController.FlyDirection.Down,landingZone.position);
    }

    public void TakeOff()
    {
        StartCoroutine(OpenDoor(1f, true));
        sdVelocity = Vector3.zero;
        Flying(RoboController.FlyDirection.Up, skyZone.position);
    }


    public void Flying(RoboController.FlyDirection direction, Vector3 destination)
    {
        switch (direction)
        {
            case RoboController.FlyDirection.Invalid:
                break;
            case RoboController.FlyDirection.Stay:
                break;
            case RoboController.FlyDirection.Up:
                StartCoroutine(FlyingThread(destination));
                break;
            case RoboController.FlyDirection.Down:
                StartCoroutine(FlyingThread(destination,delegate { StartCoroutine(OpenDoor(1.5f)); GameManager.Instance.StartGame(); }));
                break;
            default:
                break;
        }
    }

    private Vector3 sdVelocity;
    private enum Method
    {
        TimeSmooth,
        SmoothDamp,
        Lerp
    }
    [SerializeField] Method moveMethod;
    private IEnumerator FlyingThread(Vector3 destination, Action callback = null)
    {
        switch (moveMethod)
        {
            case Method.TimeSmooth:
                float timer = 0f;
                float percentage;
                Vector3 originPosition = transform.position;
                while (true)
                {
                    timer += Time.deltaTime;
                    if(timer >= flyingDuration)
                    {
                        percentage = 1;
                        transform.position = destination;
                        break;
                    }
                    percentage = timer / flyingDuration;
                    transform.position = Vector3.Lerp(originPosition, destination, percentage);
                    yield return null;
                }
                break;
            case Method.SmoothDamp:
                while (true)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, destination, ref sdVelocity, flyingDuration);
                    if (Vector3.Distance(transform.position, destination) < 0.1f)
                    {
                        break;
                    }
                    yield return null;
                }
                break;
            case Method.Lerp:
                break;
            default:
                break;
        }
        callback?.Invoke();
    }

    private IEnumerator OpenDoor(float duration,bool isClose = false)
    {
        float timer = 0f;
        float percentage;
        while (true)
        {
            timer += Time.deltaTime;
            percentage = timer / duration;
            if (isClose)
            {
                doorOpen = 1 - percentage;
            }
            else
            {
                doorOpen = percentage;
            }

            if (percentage >= 1)
            {
                doorOpen = isClose ? 0 : 1;
                yield break;
            }
            yield return null;
        }
    }

    public void FixEngine(int index,bool value)
    {
        switch (index)
        {
            case 0:
                engine1 = value;
                break;
            case 1:
                engine2 = value;
                break;
            case 2:
                engine3 = value;
                break;
            default:
                break;
        }
        if(engine1 && engine2 && engine3)
        {
            broken = false;
            OnEngineFixed?.Invoke();
        }
    }
}
