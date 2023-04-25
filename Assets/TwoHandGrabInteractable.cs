using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    public enum TwoHandRotationType {None,First,Second};
    public TwoHandRotationType twoHandRotationType;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }
        return targetRotation;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("second hand grab");
        secondInteractor = interactor;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("second hand release");
        secondInteractor = null;
    }

    // protected override void OnSelectEnter(XRBaseInteractor interactor)
    // {
    //     Debug.Log("first grab enter");
    //     base.OnSelectEnter(interactor);
    //     attachInitialRotation = interactor.attachTransform.localRotation;
    // }

    // protected override void OnSelectExit(XRBaseInteractor interactor)
    // {
    //     Debug.Log("first grab exit");
    //     base.OnSelectExit(interactor);
    //     secondInteractor = null;
    //     interactor.attachTransform.localRotation = attachInitialRotation;
    // }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
