using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrip : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public float speed;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateHand();
    }

    private void AnimateHand()
    {
        if(gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            anim.SetFloat("Grip", gripCurrent);
        }
        if(triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            anim.SetFloat("Trigger", triggerCurrent);
        }
    }

    public void SetGrip(float value)
    {
        gripTarget = value; 
    }

    public void SetTrigger(float value)
    {
        triggerTarget = value;
    }
}
