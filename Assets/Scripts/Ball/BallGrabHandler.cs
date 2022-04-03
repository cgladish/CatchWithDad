using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallGrabHandler : MonoBehaviour
{
    public bool isHeldByPlayer = false;

    private XRGrabInteractable m_XRGrabInteractable;
    private Rigidbody m_Rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        m_XRGrabInteractable = GetComponent<XRGrabInteractable>();
        m_Rigidbody = GetComponent<Rigidbody>();
        OnDadGrabRelease(); // Start off not being held
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSelectEntered() {
        isHeldByPlayer = true;
    }

    public void OnSelectExited() {
        isHeldByPlayer = false;
    }

    public void OnDadGrab() { 
        m_XRGrabInteractable.enabled = false;
        m_Rigidbody.isKinematic = true;
        gameObject.layer = Constants.LAYER_BALL_IN_HAND;
    }

    public void OnDadGrabRelease() {
        m_XRGrabInteractable.enabled = true;
        m_Rigidbody.isKinematic = false;
        gameObject.layer = Constants.LAYER_BALL;
    }
}
