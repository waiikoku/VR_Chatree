using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
public class Cooker : MonoBehaviour
{
	[SerializeField] private XRSocketInteractor socketCook;
	[SerializeField] private Image cookImage;
	private MeatController currentSnapMeat;
	[SerializeField] private AudioSource cookingSFX;
	[SerializeField] private Transform lookAtConstraint;
	private Transform targetCamera;
    private void Start()
	{
		socketCook.selectEntered.AddListener(OnSnapped);
		socketCook.selectExited.AddListener(OnUnsnap);
		targetCamera = Camera.main.transform;
	}

    private void FixedUpdate()
    {
		if (targetCamera == null) return;
		lookAtConstraint.LookAt(targetCamera);
    }

    private void LateUpdate()
	{
		if (currentSnapMeat == null) return;
		cookImage.fillAmount = currentSnapMeat.cookingProgress / 100f;
	}

	private void OnSnapped(SelectEnterEventArgs args)
	{
		MeatController mc = args.interactableObject.transform.GetComponent<MeatController>();
		if (mc != null)
		{
			mc.isCooking = true;
			currentSnapMeat = mc;
		}
		if (cookingSFX != null) cookingSFX.Play();
	}

	private void OnUnsnap(SelectExitEventArgs args)
	{
		MeatController mc = args.interactableObject.transform.GetComponent<MeatController>();
		if (mc != null)
		{
			mc.isCooking = false;
		}
		currentSnapMeat = null;
		cookImage.fillAmount = 0f;
		if (cookingSFX != null) cookingSFX.Stop();
	}
}
