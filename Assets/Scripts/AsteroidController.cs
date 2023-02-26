using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidController : MonoBehaviour
{
    private Transform m_transform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem flyingVFX;
    [SerializeField] private ParticleSystem impactVFX;
    private bool impact = false;
    [SerializeField] private AudioSource flySFX;
    [SerializeField] private AudioSource impactSFX;

    private void Awake()
    {
        m_transform = transform;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (impact == false) {
            rb.velocity = Vector3.zero;
            m_transform.position = collision.contacts[0].point;
            rb.isKinematic = true;
            flySFX.Stop();
            flyingVFX.Stop();
            impact = true;
            impactVFX.Play();
            impactSFX.Play();
            Destroy(gameObject, 1.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            MeatController mc = other.GetComponent<MeatController>();
            if(mc != null)
            {
                if (mc.isCooking)
                {
                    mc.InstantBurnt();
                }
            }
        }
    }
}
