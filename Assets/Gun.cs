using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunClip;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource audio))
        {
            gunAudioSource = audio;
        }
        else
        {
            gunAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }
    }

    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);
    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);

    private void TriggerPulled(ActivateEventArgs arg0)
    {
        arg0.interactor.GetComponent<XRBaseController>().SendHapticImpulse(1f, .1f);
        gunAudioSource.PlayOneShot(gunClip);
        FireRaycastIntoScene();
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Debug.Log("Target Hit!");
        }
    }
}
