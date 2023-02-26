using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadioController : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable interactable;
    [SerializeField] private bool toggle = false;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private ParticleSystem noteParticle;
    private void Start()
    {
        interactable.activated.AddListener(OnActivate);
    }

    private void OnActivate(ActivateEventArgs args)
    {
        toggle = !toggle;
        if(musicPlayer.isPlaying)
        {
            if(toggle == false)
            {
                musicPlayer.Stop();
                noteParticle.Stop();
            }
        }
        else
        {
            if (toggle == true)
            {
                musicPlayer.Play();
                noteParticle.Play();
            }
        }
    }
}
