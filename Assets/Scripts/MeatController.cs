using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeatController : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable interactable;
    [SerializeField] private MeshRenderer rend;
    private Material mat;
    [SerializeField] private string shaderKeyword = "_Percentage";
    [SerializeField] private string shaderKeyword_MiC = "_MinCook";
    [SerializeField] private string shaderKeyword_MaC = "_MaxCook";

    public bool isCooking = false;
    public float cookDuration = 10f;
    public float cookingSpeed = 1f;
    public float cookingProgress { get; private set; }
    public float minCook = 0.6f;
    public float maxCook = 0.8f;
    private void Start()
    {
        cookingSpeed = (1 / cookDuration) * 100f;
        mat = rend.material;
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnUngrab);
        mat.SetFloat(shaderKeyword_MiC, minCook);
        mat.SetFloat(shaderKeyword_MaC, maxCook);
    }

    private void LateUpdate()
    {
        if (isCooking == false) return;
        cookingProgress += cookingSpeed * Time.deltaTime;
        cookingProgress = Mathf.Clamp(cookingProgress, 0f, 100f);
        mat.SetFloat(shaderKeyword, cookingProgress / 100f);
    }

    public void InstantBurnt()
    {
        isCooking = false;
        cookingProgress = 100;
        mat.SetFloat(shaderKeyword, cookingProgress / 100f);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {

    }

    private void OnUngrab(SelectExitEventArgs args)
    {

    }

    public void Deactivate()
    {
        interactable.selectEntered.RemoveAllListeners();
        interactable.selectExited.RemoveAllListeners();
    }
}
