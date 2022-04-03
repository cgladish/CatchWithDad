using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallGrabHandler : MonoBehaviour
{
    public bool inFlightAfterPlayerThrow = false;

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
        inFlightAfterPlayerThrow = false;
    }

    public void OnSelectExited() {
        inFlightAfterPlayerThrow = true;
    }

    public void OnCollisionEnter(Collision collision) {
        if (
            collision.gameObject.tag != Constants.TAG_CONTROLLER_LEFT_HAND
            && collision.gameObject.tag != Constants.TAG_CONTROLLER_RIGHT_HAND
        ) {
            inFlightAfterPlayerThrow = false;
        }
    }

    public void OnDadGrab() {
        inFlightAfterPlayerThrow = false;
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
