using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Big")]
    [SerializeField] private Transform bigAsteroid;
    [SerializeField] private Rigidbody baRB;
    [SerializeField] private Transform targetImpact;
    public float distance;
    public float duration;
    public Vector3 velocity;
    private float bigTimer;
    private bool hasStart = false;
    private bool hasImpact = false;
    private Material tempSkybox;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color targetColor;
    private float percentage;
    [SerializeField] private AudioSource flyMeteorSFX;
    [SerializeField] private AudioSource impactSFX;
    public event System.Action OnImpact;
    [Header("Small")]
    [SerializeField] private GameObject[] smallAsteroids;
    [SerializeField] private Transform[] targetDestinations;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject endGameUI;
    [Header("Attributes")]
    [SerializeField] private float height = 200f;
    [SerializeField] private Vector3 areaSize = Vector3.one;
    [SerializeField] private Vector3 minArea = Vector3.one;
    [SerializeField] private Vector3 maxArea = Vector3.one;
    [SerializeField] private float spawnChance = 50f; //of 100%
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float minimumSpeed = 10f;
    [SerializeField] private float maximumSpeed = 20f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, height, 0), areaSize);
    }

    private void Start()
    {
        StartCoroutine(MeteorRain());
        Vector3 direction = targetImpact.position - bigAsteroid.position;
        bigAsteroid.LookAt(targetImpact);
        distance = direction.magnitude;
        velocity = direction.normalized * (distance / duration);
        tempSkybox = RenderSettings.skybox;
        tempSkybox.SetColor("_TintColor", Color.Lerp(baseColor, targetColor, 0));
        OnImpact += delegate { endGameUI.SetActive(true); GameManager.Instance.EndGame(true); };
    }

    private void FixedUpdate()
    {
        if (hasStart == false) return;
        if (hasImpact) return;
        baRB.velocity = velocity;
        bigTimer += Time.deltaTime;
        percentage = bigTimer / duration;
        tempSkybox.SetColor("_TintColor", Color.Lerp(baseColor, targetColor, percentage));
        if(bigTimer > duration)
        {
            hasImpact = true;
            baRB.velocity = Vector3.zero;
            baRB.isKinematic = true;
            OnImpact?.Invoke();
            flyMeteorSFX.Stop();
            impactSFX.Play();
        }
    }

    private IEnumerator MeteorRain()
    {
        while (GameManager.Instance.IsStart == false)
        {
            yield return null;
        }
        hasStart = true;
        baRB.isKinematic = false;
        baRB.useGravity = true;
        flyMeteorSFX.Play();
        while (true)
        {
            int randomIndex = Random.Range(0, targetDestinations.Length);
            Vector3 randomPosition = new Vector3(Random.Range(minArea.x, maxArea.x), Random.Range(minArea.y, maxArea.y) + height, Random.Range(minArea.z, maxArea.z));
            GameObject go = Instantiate(smallAsteroids[Random.Range(0,smallAsteroids.Length)],randomPosition , Quaternion.identity);
            if(go.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 direction = targetDestinations[randomIndex].position - go.transform.position;
                go.transform.LookAt(direction);
                rb.AddForce(direction.normalized * Random.Range(minimumSpeed, maximumSpeed), ForceMode.Acceleration);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void StopMeteor()
    {
        if (hasStart)
        {
            hasStart = false;
            baRB.useGravity = false;
            baRB.isKinematic = true;
            StopAllCoroutines();
        }
    }
}
