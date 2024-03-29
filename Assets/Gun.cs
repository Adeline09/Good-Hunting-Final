using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask deerLayer;
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunClip;
    public GameObject muzzle;
    public float offset = -0.5f;
    public float VerticleOffset = 0.1f;
    public GameObject Deer;
    private Wander Wander;
    private float hitTime = 0.0f;

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
        Instantiate(muzzle, transform.position + transform.forward*offset + transform.up*VerticleOffset, muzzle.transform.rotation);
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Debug.Log("Target Hit!");
        }
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, deerLayer))
        {
            hitTime += 1.0f;
        }
        if (hitTime == 1.0f)
        {
            Destroy(Deer, 10.0f);
            Wander = Deer.GetComponent<Wander>();
            Wander.enabled = false;
            Transform deerChild = Deer.transform.GetChild(0);
            Animator animator = deerChild.GetComponent<Animator>();
            animator.enabled = false;
            Deer.transform.Rotate(0f, 0f, 90f);
            Deer.transform.Translate(-1f, 0f, 0f);
        }
    }
}
