using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 1f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AsteroidManager astm;
    private void Start()
    {
        astm.OnImpact += delegate { StartCoroutine(Shaking()); };
    }

    private IEnumerator Shaking()
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.localPosition = startPosition;
    }
}
