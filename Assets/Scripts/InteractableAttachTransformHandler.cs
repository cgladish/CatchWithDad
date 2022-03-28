using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableAttachTransformHandler : MonoBehaviour
{
    public Transform leftHandAttachTransform;
    public Transform rightHandAttachTransform;
    private XRGrabInteractable m_XRGrabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        m_XRGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectEntered(SelectEnterEventArgs eventArgs)
    {
        string interactorObjectTag = eventArgs.interactorObject.transform.gameObject.tag;
        if (interactorObjectTag == Constants.TAG_CONTROLLER_LEFT_HAND) {
            m_XRGrabInteractable.attachTransform = leftHandAttachTransform;
        } else if (interactorObjectTag == Constants.TAG_CONTROLLER_RIGHT_HAND) {
            m_XRGrabInteractable.attachTransform = rightHandAttachTransform;
        }
    }

    public void OnSelectExited(SelectExitEventArgs eventArgs)
    {
    }
}
